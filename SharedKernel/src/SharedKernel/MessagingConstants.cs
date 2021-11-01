namespace SharedKernel
{
    public static class MessagingConstants
    {
        public static class Exchanges
        {
            public const string FRONTDESK_VETCLINICPUBLIC_EXCHANGE = "frontdesk-vetclinicpublic";
        }

        public static class Queues
        {
            public const string FDVCP_FRONTDESK_IN = "fdvcp-frontdesk-in";
            public const string FDVCP_VETCLINICPUBLIC_IN = "fdvcp-vetclinicpublic-in";
        }
    }
}
