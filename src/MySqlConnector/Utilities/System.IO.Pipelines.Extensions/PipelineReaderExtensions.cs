// copied from https://github.com/aspnet/SignalR/blob/3acd29ec6f3af185125c276fa0a7a43b4a1c5d5f/src/Microsoft.AspNetCore.Sockets.Client.Http/Internal/PipelineReaderExtensions.cs

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    internal static class PipelineReaderExtensions
    {
        public static async Task CopyToAsync(this PipeReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            // TODO: Use bufferSize argument
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;
                try
                {
                    if (inputBuffer.IsEmpty && result.IsCompleted)
                    {
                        return;
                    }

                    await inputBuffer.CopyToAsync(stream);
                }
                finally
                {
                    input.AdvanceTo(inputBuffer.End);
                }
            }
        }
    }
}
