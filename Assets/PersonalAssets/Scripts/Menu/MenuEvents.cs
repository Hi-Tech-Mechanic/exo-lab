using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public ConfirmationWindow confirmWindow;

    public void ExitTheApplication()
    {
        this.confirmWindow.Show("Вы уверены, что хотите выйти?", ExitTheApplicationEvent);
    }

    public void ExitTheApplicationEvent()
    {
        Application.Quit();
    }
}
