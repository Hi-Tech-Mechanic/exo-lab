using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public ConfirmationWindow confirmWindow;

    public void ExitTheApplication()
    {
        this.confirmWindow.Show("Вы уверены, что хотите выйти?", ExitTheApplicationEvent);
    }

    private void ExitTheApplicationEvent()
    {
        Application.Quit();
    }
}
