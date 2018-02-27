// https://github.com/aspnet/SignalR/blob/3acd29ec6f3af185125c276fa0a7a43b4a1c5d5f/src/Microsoft.AspNetCore.Sockets.Client.Http/Internal/StreamPipeConnection.cs

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    internal class StreamPipeConnection : IDuplexPipe
    {
        public StreamPipeConnection(PipeOptions options, Stream stream)
        {
            Input = CreateReader(options, stream);
            Output = CreateWriter(options, stream);
        }

        public PipeReader Input { get; }

        public PipeWriter Output { get; }

        public void Dispose()
        {
            Input.Complete();
            Output.Complete();
        }

        public static PipeReader CreateReader(PipeOptions options, Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new NotSupportedException();
            }

            var pipe = new Pipe(options);
            var ignore = stream.CopyToEndAsync(pipe.Writer);

            return pipe.Reader;
        }

        public static PipeWriter CreateWriter(PipeOptions options, Stream stream)
        {
            if (!stream.CanWrite)
            {
                throw new NotSupportedException();
            }

            var pipe = new Pipe(options);
            var ignore = pipe.Reader.CopyToEndAsync(stream);

            return pipe.Writer;
        }
    }
}
