namespace AI.Devs.OwnApi.Utils.Consts;

public class OpenAiApi
{
    public class Roles
    {
        public const string
            User = "user",
            System = "system",
            Assistant = "assistant"
            ;
    }

    public class ModelsGpt
    {
        public const string 
            Gpt35Turbo = "gpt-3.5-turbo",
            Gpt4 = "gpt-4",
            Gpt4Turbo = "gpt-4-turbo"
            ;
    }

    public class TranscriptionModels
    {
        public const string
            Whisper1 = "whisper-1"
            ;
    }

    public class EmbeddingTechniques
    {
        public const string
            TextEmbeddingAda002 = "text-embedding-ada-002"
            ;
    }
}
