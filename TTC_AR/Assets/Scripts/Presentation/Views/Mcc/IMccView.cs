using System.Collections.Generic;

public interface IMccView
{
    void ShowLoading(string title = "Đang tải dữ liệu");
    void HideLoading();
    void ShowError(string message);
    void ShowSuccess();
    void DisplayList(List<MccInformationModel> models);
    void DisplayFieldDeviceList(List<FieldDeviceInformationModel> models);
    void DisplayDetail(MccInformationModel model);
    void DisplayCreateResult(bool success);
    void DisplayUpdateResult(bool success);
    void DisplayDeleteResult(bool success);
}