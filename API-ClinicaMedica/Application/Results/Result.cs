﻿namespace API_ClinicaMedica.Application.Results;

public record Result
{
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    
    }

    public record Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true, null) => Value = value;
        private Result(Error error) : base(false, error) { }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Failure(Error error) => new(error);
    }
