namespace HslAuth
{
    public static class HslAuth
    {
        public static bool Auth()
        {
            bool isAuthed = HslCommunication.Authorization.SetAuthorizationCode("7498b08c-6426-4350-a860-614db4d6bdcf");
            return isAuthed;
        }
    }
}
