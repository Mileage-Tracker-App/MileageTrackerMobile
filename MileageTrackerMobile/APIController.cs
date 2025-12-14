using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MileageTrackerMobile.APIController
{
    public sealed class Result<T>
    {
        public bool Success { get; init; }
        public HttpStatusCode StatusCode { get; init; }
        public string? Error { get; init; }
        public T? Value { get; init; }
        public string? RawBody { get; init; }

        public static Result<T> Ok(T? value, HttpStatusCode status, string? raw = null) => new()
        {
            Success = true,
            StatusCode = status,
            Value = value,
            RawBody = raw
        };

        public static Result<T> Fail(HttpStatusCode status, string? error, string? raw = null) => new()
        {
            Success = false,
            StatusCode = status,
            Error = error,
            RawBody = raw
        };
    }

    // Client for calling the MileageTracker API from another project
    public class ApiController
    {
        // Base API URL (adjust as needed)
        public string APIROOT { get; set; } = "http://localhost:8080/api/";

        private readonly HttpClient _http;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public ApiController(HttpClient? httpClient = null, string? apiRoot = null)
        {
            _http = httpClient ?? new HttpClient();
            if (!string.IsNullOrWhiteSpace(apiRoot)) APIROOT = apiRoot!;
            if (!_http.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")))
            {
                _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        // -----------------------
        // Generic HTTP helpers
        // -----------------------
        public Task<Result<TOut>> GetAsync<TOut>(string path, CancellationToken ct = default)
            => SendAsync<object, TOut>(HttpMethod.Get, path, body: null, ct: ct);

        public Task<Result<TOut>> DeleteAsync<TOut>(string path, CancellationToken ct = default)
            => SendAsync<object, TOut>(HttpMethod.Delete, path, body: null, ct: ct);

        public Task<Result<bool>> DeleteAsync(string path, CancellationToken ct = default)
            => SendAsync<object, object>(HttpMethod.Delete, path, body: null, ct: ct)
                .ContinueWith(t =>
                {
                    var r = t.Result;
                    return r.Success
                        ? Result<bool>.Ok(true, r.StatusCode, r.RawBody)
                        : Result<bool>.Fail(r.StatusCode, r.Error, r.RawBody);
                }, ct);

        public Task<Result<TOut>> PostAsync<TIn, TOut>(string path, TIn body, CancellationToken ct = default)
            => SendAsync<TIn, TOut>(HttpMethod.Post, path, body, ct);

        public Task<Result<TOut>> PutAsync<TIn, TOut>(string path, TIn body, CancellationToken ct = default)
            => SendAsync<TIn, TOut>(HttpMethod.Put, path, body, ct);

        // Core sender
        private async Task<Result<TOut>> SendAsync<TIn, TOut>(HttpMethod method, string path, TIn? body, CancellationToken ct)
        {
            var url = Combine(APIROOT, path);
            using var req = new HttpRequestMessage(method, url);

            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, JsonOptions);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage resp;
            try
            {
                resp = await _http.SendAsync(req, HttpCompletionOption.ResponseContentRead, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result<TOut>.Fail(0, ex.Message);
            }

            string raw = await SafeReadAsStringAsync(resp).ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                // Return error result with raw body for diagnostics
                return Result<TOut>.Fail(resp.StatusCode, $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}", raw);
            }

            if (typeof(TOut) == typeof(string))
            {
                // allow callers to get raw string
                object boxed = raw;
                return Result<TOut>.Ok((TOut)boxed, resp.StatusCode, raw);
            }

            if (typeof(TOut) == typeof(byte[]))
            {
                var bytes = await resp.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
                object boxed = bytes;
                return Result<TOut>.Ok((TOut)boxed, resp.StatusCode, raw);
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return Result<TOut>.Ok(default, resp.StatusCode, raw);
            }

            try
            {
                var value = JsonSerializer.Deserialize<TOut>(raw, JsonOptions);
                return Result<TOut>.Ok(value, resp.StatusCode, raw);
            }
            catch (Exception ex)
            {
                return Result<TOut>.Fail(resp.StatusCode, $"Failed to deserialize {typeof(TOut).Name}: {ex.Message}", raw);
            }
        }

        private static async Task<string> SafeReadAsStringAsync(HttpResponseMessage resp)
        {
            try { return await resp.Content.ReadAsStringAsync().ConfigureAwait(false); }
            catch { return string.Empty; }
        }

        private static string Combine(string root, string path)
        {
            if (string.IsNullOrWhiteSpace(root)) return path;
            if (string.IsNullOrWhiteSpace(path)) return root.TrimEnd('/') + "/";
            return root.TrimEnd('/') + "/" + path.TrimStart('/');
        }
        
        // -----------------------
        // Sessions endpoints
        // -----------------------
        public Task<Result<List<TSession>>> GetAllSessionsAsync<TSession>(CancellationToken ct = default)
            => GetAsync<List<TSession>>("sessions", ct);

        public Task<Result<TSession>> GetSessionAsync<TSession>(int sessionId, CancellationToken ct = default)
            => GetAsync<TSession>($"sessions/{sessionId}", ct);

        public Task<Result<TSession>> CreateSessionAsync<TSession>(CancellationToken ct = default)
            => PostAsync<object, TSession>("sessions", body: null!, ct);

        public Task<Result<bool>> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
            => DeleteAsync($"sessions/{sessionId}", ct);

        // -----------------------
        // Logs endpoints
        // -----------------------
        public Task<Result<List<TLog>>> GetLogsAsync<TLog>(int sessionId, CancellationToken ct = default)
            => GetAsync<List<TLog>>($"sessions/{sessionId}/logs", ct);

        public Task<Result<TLog>> GetLogAsync<TLog>(int sessionId, int logId, CancellationToken ct = default)
            => GetAsync<TLog>($"sessions/{sessionId}/logs/{logId}", ct);

        public Task<Result<TLog>> CreateLogAsync<TLog>(int sessionId, TLog log, CancellationToken ct = default)
            => PostAsync<TLog, TLog>($"sessions/{sessionId}/logs", log, ct);

        public Task<Result<TLog>> UpdateLogAsync<TLog>(int sessionId, int logId, TLog log, CancellationToken ct = default)
            => PutAsync<TLog, TLog>($"sessions/{sessionId}/logs/{logId}", log, ct);

        public Task<Result<bool>> DeleteLogAsync(int sessionId, int logId, CancellationToken ct = default)
            => DeleteAsync($"sessions/{sessionId}/logs/{logId}", ct);

        // -----------------------
        // LogItems endpoints
        // -----------------------
        public Task<Result<List<TLogItem>>> GetLogItemsAsync<TLogItem>(int sessionId, int logId, CancellationToken ct = default)
            => GetAsync<List<TLogItem>>($"sessions/{sessionId}/logs/{logId}/items", ct);

        public Task<Result<TLogItem>> GetLogItemAsync<TLogItem>(int sessionId, int logId, int itemId, CancellationToken ct = default)
            => GetAsync<TLogItem>($"sessions/{sessionId}/logs/{logId}/items/{itemId}", ct);

        public Task<Result<TLogItem>> CreateLogItemAsync<TLogItem>(int sessionId, int logId, TLogItem logItem, CancellationToken ct = default)
            => PostAsync<TLogItem, TLogItem>($"sessions/{sessionId}/logs/{logId}/items", logItem, ct);

        public Task<Result<TLogItem>> UpdateLogItemAsync<TLogItem>(int sessionId, int logId, int itemId, TLogItem logItem, CancellationToken ct = default)
            => PutAsync<TLogItem, TLogItem>($"sessions/{sessionId}/logs/{logId}/items/{itemId}", logItem, ct);

        public Task<Result<bool>> DeleteLogItemAsync(int sessionId, int logId, int itemId, CancellationToken ct = default)
            => DeleteAsync($"sessions/{sessionId}/logs/{logId}/items/{itemId}", ct);
    }
}