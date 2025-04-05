namespace Infrastructure.Repositories.Queries
{
    public static class SampleMessageQueries
    {
        public const string GetAll = """
            SELECT 
                MESSAGE_ID as MessageId
                MESSAGE_DATE as Date
                FROM SAMPLE_MESSAGES
            ORDER BY DATE
            """;

        public const string Add = """
            INSERT INTO SAMPLE_MESSAGES
            (MESSAGE_ID, MESSAGE_DATE)
            VALUES
            (@MessageId, @MessageDate)
            """;
    }
}
