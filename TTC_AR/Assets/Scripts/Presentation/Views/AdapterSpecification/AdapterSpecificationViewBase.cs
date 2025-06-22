using System.Collections.Generic;

public interface IAdapterSpecificationView
{
    void ShowLoading(string title = "Đang tải dữ liệu");
    void HideLoading();
    void ShowError(string message);
    void ShowSuccess(string message);
    void DisplayList(List<AdapterSpecificationModel> models);
    void DisplayDetail(AdapterSpecificationModel model);
    void DisplayCreateResult(bool success);
    void DisplayUpdateResult(bool success);
    void DisplayDeleteResult(bool success);
}