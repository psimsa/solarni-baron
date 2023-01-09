namespace TestHelpers;

public static class AssertWrapper
{
    public static void AssertAll(params Action[] actions)
    {
        if (actions == null)
        {
            throw new ArgumentNullException(nameof(actions));
        }

        Task.WaitAll(actions.Select(WrapTask).ToArray());
    }

    private static Task WrapTask(Action task)
    {
        try
        {
            task();
        }
        catch (Exception ex) when (ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
        return Task.CompletedTask;
    }
}
