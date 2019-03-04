namespace MVCWeb.Libraries.Pager
{
    /// <summary>
    /// Pager model
    /// </summary>
    public interface IPager
    {
        int PageCount { get; }

        int CurrentPage { get; }

        int StartDisplayedPage { get; }

        int EndDisplayedPage { get; }

        /// <summary>
        /// Get address for page
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <returns>Page address</returns>
        string GetLinkForPage(int pageIndex);
    }
}