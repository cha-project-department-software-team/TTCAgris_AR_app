using System.Collections;
using System.Collections.Generic;

public interface ILoginView
{
    void ShowLoading(string title = "Đang tải dữ liệu");
    void HideLoading();
    void ShowError(string message);
    void ShowSuccess();
   
}