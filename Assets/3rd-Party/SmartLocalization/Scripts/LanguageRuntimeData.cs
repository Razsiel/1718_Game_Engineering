//LanguageRuntimeData.cs
//
// Written by Niklas Borglund and Jakob Hillerström
//
namespace SmartLocalization
{
/// <summary>
/// An information class containing runtime data.
/// </summary>
public static class LanguageRuntimeData 
{
	//todo:: load this from a file generated by the editor classes
	static string AvailableCulturesFileName	= "AvailableCultures";
	static string rootLanguageName			= "Language";

	static string AudioFileFolderName		= "AudioFiles";
	static string TexturesFolderName		= "Textures";
	static string PrefabsFolderName			= "Prefabs";
	static string TextAssetsFolderName		= "TextAssets";
	static string FontsFolderName			= "Fonts";


#region Lookups & Paths
	/// <summary>
	/// Gets the relative language file path for a certain language. Mostly used with Resources.Load as a TextAsset.
	/// </summary>
	/// <param name="languageCode">The language code of the culture</param>
	/// <returns>The relative language file path</returns>
	public static string LanguageFilePath(string languageCode)
	{
		return rootLanguageName + "." + languageCode;
	}

	/// <summary>
	/// Gets the relative file path for the AvailableCultures xml. Use with Resources.Load as a TextAsset.
	/// </summary>
	/// <returns>The AvailableCultures.xml relative file path</returns>
	public static string AvailableCulturesFilePath()
	{
		return AvailableCulturesFileName;
	}

	/// <summary>
	/// Gets the relative folder path for the audio files by a specific language 
	/// </summary>
	public static string AudioFilesFolderPath(string languageCode)
	{
		return languageCode + "/" + AudioFileFolderName;
	}

	/// <summary>
	/// Gets the relative folder path for the texture files by a specific language 
	/// </summary>
	public static string TexturesFolderPath(string languageCode)
	{
		return languageCode + "/" + TexturesFolderName;
	}

	public static string TextAssetsFolderPath(string languageCode)
	{
		return languageCode + "/" + TextAssetsFolderName;
	}
	public static string FontsFolderPath(string languageCode)
	{
		return languageCode + "/" + FontsFolderName;
	}

	public static string PrefabsFolderPath(string languageCode)
	{
		return languageCode + "/" + PrefabsFolderName;
	}

#endregion
}
}