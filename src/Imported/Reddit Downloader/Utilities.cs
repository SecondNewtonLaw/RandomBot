using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reddit.PostDownloader.Extensions;

public static class Extensions
{
    /// <summary>
    /// Removes characters that are not allowed in file names.
    /// </summary>
    /// <returns>a new string without illegal characters.</returns>
    public static async Task<string> SanitizeString(this string fileName, CancellationToken tken = new())
    {
        char[] stringChars = fileName.ToCharArray();

        Func<char, bool> sanitizeStringFunc
            = new(inChar =>
                    {
                        // Iterate though the illegal chars in filenames, if match return false.
                        for (int i = 0; i < Path.GetInvalidFileNameChars().Length; i++)
                        {
                            if (inChar == Path.GetInvalidFileNameChars()[i])
                                return false;

                            if (tken.IsCancellationRequested)
                                throw new TaskCanceledException("The ongoing task was cancelled.");
                        }
                        // If no match, return true
                        return true;
                    });

        IEnumerable<char> trueString = await Task.Factory.StartNew(() => stringChars.Where(sanitizeStringFunc), tken, TaskCreationOptions.PreferFairness, TaskScheduler.Default);

        StringBuilder result = new();
        result.Append(trueString.ToArray());

        return result.ToString();
    }

    /// <summary>
    /// Flushes, Disposes and closes a stream.
    /// </summary>
    /// <param name="inStream">Stream to destroy</param>
    public static void DestroyStream(this Stream inStream)
    {
        inStream.Flush();
        inStream.Dispose();
        inStream.Close();
    }

    /// <summary>
    /// Flushes, Disposes and closes a collection of <see cref="ICollection"/> streams.
    /// </summary>
    /// <param name="inStreams">Streams to destroy</param>
    public static async Task DestroyStreamsAsync(this ICollection<Stream> inStreams, TaskScheduler sched = null!, CancellationToken tken = new())
    {
        if (sched is null)
            sched = TaskScheduler.Current;

        List<Task> taskList = new();
        for (int i = 0; i < inStreams.Count; i++)
        {
            int i2 = i;
            taskList.Add(Task.Factory.StartNew(() =>
            {
                inStreams.ElementAt(i2).Flush();
                inStreams.ElementAt(i2).Dispose();
                inStreams.ElementAt(i2).Close();
            }, tken, TaskCreationOptions.HideScheduler, sched));
        }

        while (taskList.Count > 0)
        {
            Task completed = await Task.WhenAny(taskList); // Completed
            taskList.Remove(completed); // Remove completed.
        }
    }
}