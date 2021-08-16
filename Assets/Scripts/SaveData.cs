[System.Serializable]
public class SaveData
{
    /// <summary>
    /// The high score for the player
    /// </summary>
    public int highScore;
    public bool keyBoardControls;

    /// <summary>
    /// Creates a sava data object with a high score of 0
    /// </summary>
    public SaveData()
    {
        highScore = 0;
        keyBoardControls = false;
    }

}
