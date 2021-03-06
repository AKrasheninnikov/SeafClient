﻿using System.Net;
using System.Net.Http;

namespace SeafClient.Types
{
    /// <summary>
    ///     Possible status codes returned by the Seafile API.
    ///     These are in fact HTTP status codes
    /// </summary>
    public enum SeafErrorCode
    {
        NoDetails = -1,
        InvalidCredentials = 1,
        PathDoesNotExist,
        FileNotFound,
        EncryptedLibrary_PasswordNotProvided,
        InvalidToken,
        OutOfQuota,
        NotEnoughPermissions,
        InvalidLibraryPassword,
        LibraryIsNotEncrypted,
        ProtocolError,
        TooManyRequests
    }

    /// <summary>
    ///     Represents a seafile error which consists
    ///     of a HTTP status code and a context-sensitive seafile error code if available
    /// </summary>
    public class SeafError
    {
        private readonly string _httpStatusDescription;

        public SeafError(HttpStatusCode httpStatusCode, SeafErrorCode seafErrorCode)
        {
            HttpStatusCode = httpStatusCode;
            SeafErrorCode = seafErrorCode;

            var respond = new HttpResponseMessage(HttpStatusCode);
            _httpStatusDescription = respond.ReasonPhrase;
        }

        /// <summary>
        ///     The HTTP status code received from the seafile server
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     A more detailed error message in the seafile context if available
        /// </summary>
        public SeafErrorCode SeafErrorCode { get; }

        public string GetErrorMessage()
        {
            switch (SeafErrorCode)
            {
                case SeafErrorCode.InvalidCredentials:
                    return "The given login credentials are invalid.";
                case SeafErrorCode.PathDoesNotExist:
                    return "The path does not exist.";
                case SeafErrorCode.FileNotFound:
                    return "The file does not exist.";
                case SeafErrorCode.EncryptedLibrary_PasswordNotProvided:
                    return "The library is enrypted but password was not provided.";
                case SeafErrorCode.InvalidToken:
                    return "The token is invalid.";
                case SeafErrorCode.OutOfQuota:
                    return "The user ran out of quota.";
                case SeafErrorCode.NotEnoughPermissions:
                    return "Not enough permissions to execute this request.";
                case SeafErrorCode.InvalidLibraryPassword:
                    return "The provided password is invalid.";
                case SeafErrorCode.LibraryIsNotEncrypted:
                    return "The library decryption request failed, since the library is not encrypted.";
                case SeafErrorCode.TooManyRequests:
                    return "The server returned TooManyRequests. Please wait before continuing sending requests.";
                default:
                    // print the http status code
                    return _httpStatusDescription;
            }
        }
    }
}