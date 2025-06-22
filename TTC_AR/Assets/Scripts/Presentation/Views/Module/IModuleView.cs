using System.Collections.Generic;

public interface IModuleView
{
    void ShowLoading(string title = "Loading...");
    void HideLoading();
    void ShowError(string message);
    void ShowSuccess(string message);
    void DisplayList(List<ModuleInformationModel> models);
    void DisplayDetail(ModuleInformationModel model);
    void DisplayCreateResult(bool success);
    void DisplayUpdateResult(bool success);
    void DisplayDeleteResult(bool success);
}