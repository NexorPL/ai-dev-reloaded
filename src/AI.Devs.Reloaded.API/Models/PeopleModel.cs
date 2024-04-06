using System.Text.Json.Serialization;

namespace AI.Devs.Reloaded.API.Models;

public sealed class PeopleModel
{
    [JsonPropertyName("imie")]
    public required string Firstname { get; set; }

    [JsonPropertyName("nazwisko")]
    public required string Lastname { get; set; }

    [JsonPropertyName("wiek")]
    public required int Age { get; set; }

    [JsonPropertyName("o_mnie")]
    public required string AboutMe { get; set; }

    [JsonPropertyName("ulubiona_postac_z_kapitana_bomby")]
    public required string FavoriteCharacterFromKapitanBomba { get; set; }

    [JsonPropertyName("ulubiony_serial")]
    public required string FavoriteSeries { get; set; }

    [JsonPropertyName("ulubiony_film")]
    public required string FavoriteMovie { get; set; }

    [JsonPropertyName("ulubiony_kolor")]
    public required string FavoriteColour { get; set; }

    internal string FullName { get => Firstname + " " + Lastname; }
}
