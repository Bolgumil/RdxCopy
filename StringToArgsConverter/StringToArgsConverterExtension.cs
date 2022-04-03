namespace StringToArgsConverter
{
    public static class StringToArgsConverterExtension
    {
        /// <summary>
        /// Converts raw string into string[], like it is done with CLI input.
        /// </summary>
        /// <param name="argsRaw">String to be converted</param>
        /// <returns>Args</returns>
        public static string[] ToArgs(this string argsRaw)
        {
            char[] parmChars = argsRaw.Trim().ToCharArray();
            bool inDoubleQuote = false;
            bool escaped = false;
            bool lastSplitted = false;
            bool justSplitted = false;
            bool lastQuoted = false;
            bool justQuoted = false;

            int i, j;

            for (i = 0, j = 0; i < parmChars.Length; i++, j++)
            {
                parmChars[j] = parmChars[i];

                if (!escaped)
                {
                    if (parmChars[i] == '^')
                    {
                        escaped = true;
                        j--;
                    }
                    else if (parmChars[i] == '"')
                    {
                        inDoubleQuote = !inDoubleQuote;
                        parmChars[j] = '\n';
                        justSplitted = true;
                        justQuoted = true;
                    }
                    else if (!inDoubleQuote && parmChars[i] == ' ')
                    {
                        parmChars[j] = '\n';
                        justSplitted = true;
                    }

                    if (justSplitted && lastSplitted && (!lastQuoted || !justQuoted))
                        j--;

                    lastSplitted = justSplitted;
                    justSplitted = false;

                    lastQuoted = justQuoted;
                    justQuoted = false;
                }
                else
                {
                    escaped = false;
                }
            }

            if (lastQuoted)
                j--;

            return (new string(parmChars, 0, j))
                .Trim(new[] { '\n' })
                .Split(new[] { '\n' });
        }

        // Comments for my reviewer (@artemvalmus):
        //
        // As this was far not a trivial problem I obviously looked it up and tried to select the most appropriate one.
        // Listing here the ones I have tried with some comments:
        // 
        // 1) https://blog.actorsfit.com/a?ID=00950-5a2d6734-7fdb-4b97-9f38-01954399eefb
        //    - I assume there should be a more convenient solution, rather than importing shell32.dll manually
        //    - Not 100% sure but this quite seems like it ties my solution to Windows.
        //
        // 2) https://stackoverflow.com/a/59131568/7653128
        //    - Failed on "
        // 
        // 3) https://stackoverflow.com/a/19725880/7653128
        //    - Turned out to be the final one, passed most tests without modification
        //    - Had to modify it a bit as it failed on a couple of things:
        //      - Leading and trailing whitespaces: Solved by Trimming the input.
        //      - Not sure why was ' considered as the CLI does not seem to work differently around them. Removed that logic.
        //      - If the input string started with a ", then the first arg was an empty string, which is incorrect.
        //        Trimming \n at the end before splitting based on them.
        //    Stopped here as this is enough for me now.
        //
        // 4) https://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
        //    - Found this later than 3), but wanted to mention as it helped me in testing.
        //    - Did not want to replace current solution as it works quite fine for now and this ones looks strange.
        //      Not tested it but it seems it handles key-value pairs.
        //    - Also it is from 2002, quite an old one.
    }
}