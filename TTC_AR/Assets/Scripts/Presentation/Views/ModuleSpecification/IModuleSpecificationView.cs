using System.Collections.Generic;

public interface IModuleSpecificationView
{
    void ShowLoading(string title = "Đang tải dữ liệu");
    void HideLoading();
    void ShowError(string message);
    void ShowSuccess(string message);
    void DisplayList(List<ModuleSpecificationModel> models);
    void DisplayDetail(ModuleSpecificationModel model);
    void DisplayCreateResult(bool success);
    void DisplayUpdateResult(bool success);
    void DisplayDeleteResult(bool success);
}