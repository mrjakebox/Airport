using Air.Models;
using Air.Pages.Edit;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Air.ViewModels
{
     public class PlaneViewModel : PropertyObservable
    {
        public ObservableCollection<PlaneModel> Planes
        {
            get => MainController.Instance.Planes;
            set
            {
                MainController.Instance.Planes = value;
                OnPropertyChanged("Planes");
            }
        }

        public PlaneViewModel()
        {
            Message = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
        }

        private ObservableCollection<AirlineModel> _airlineList;
        private ObservableCollection<AirlineModel> AirlineList
        {
            get => _airlineList;
            set
            {
                _airlineList = value;
                OnPropertyChanged("AirlineList");
            }
        }


        private PlaneModel _selectedPlane;

        public PlaneModel SelectedPlane
        {
            get => _selectedPlane;
            set
            {
                _selectedPlane = value;
                OnPropertyChanged("SelectedPlane");
            }
        }

        private string _updateDateTime;

        public string UpdateDateTime
        {
            get => _updateDateTime;
            set
            {
                _updateDateTime = "REFRESH TIME: " + value;
                OnPropertyChanged("UpdateDateTime");
            }
        }

        #region SNACKBAR
        private SnackbarMessageQueue _message;
        private bool _isSnackbarActive;

        public SnackbarMessageQueue Message
        {
            get => _message;
            set
            {
                if (_message != null)
                    IsSnackbarActive = true;
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public bool IsSnackbarActive
        {
            get => _isSnackbarActive;
            set
            {
                _isSnackbarActive = value;
                OnPropertyChanged("IsSnackbarActive");
            }
        }
        #endregion

        public ICommand RunDialogCommand { get; set; }
        public ICommand AcceptDialogCommand { get; set; }
        public ICommand CancelDialogCommand { get; set; }

        private bool _isDialogOpen;
        private object _dialogContent;
        private RelayCommand _updateCommand;
        private RelayCommand _createCommand;
        private RelayCommand _deleteCommand;

        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set
            {
                if (_isDialogOpen == value) return;
                _isDialogOpen = value;
                OnPropertyChanged("IsDialogOpen");
            }
        }

        public object DialogContent
        {
            get { return _dialogContent; }
            set
            {
                if (_dialogContent == value) return;
                _dialogContent = value;
                OnPropertyChanged("DialogContent");
            }
        }

        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand = new RelayCommand(async obj =>
                {
                    if (SelectedPlane == null)
                    { Message.Enqueue("First, select an item"); return; }
                    await GetComboListAsync();
                    DialogContent = new ProgressDialog();
                    RunDialogCommand = new AnotherCommandImplementation(RunUpdateDialog);
                    AcceptDialogCommand = new AnotherCommandImplementation(AcceptUpdateDialogAsync);
                    CancelDialogCommand = new AnotherCommandImplementation(CancelDialog);
                    RunDialogCommand.Execute(RunDialogCommand);
                }));
            }
        }
        private async Task GetComboListAsync()
        {
            DialogContent = new ProgressDialog();
            IsDialogOpen = true;
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    AirlineList = await Task.Run(() => ModelConnection.SqlConnection.Instance.Airlines(transaction).SelectListFormatAsync());
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    Message.Enqueue(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    ModelConnection.SqlConnection.Instance.Close();
                }
            }
            IsDialogOpen = false;
        }
        public RelayCommand CreateCommand
        {
            get
            {
                return _createCommand ?? (_createCommand = new RelayCommand(async obj =>
                {
                    await GetComboListAsync();
                    RunDialogCommand = new AnotherCommandImplementation(RunCreateDialog);
                    AcceptDialogCommand = new AnotherCommandImplementation(AcceptCreateDialogAsync);
                    CancelDialogCommand = new AnotherCommandImplementation(CancelDialog);
                    RunDialogCommand.Execute(RunDialogCommand);
                }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(obj =>
                {
                    if (SelectedPlane == null)
                    { Message.Enqueue("First, select an item"); return; }
                    RunDialogCommand = new AnotherCommandImplementation(RunDeleteDialog);
                    AcceptDialogCommand = new AnotherCommandImplementation(AcceptDeleteDialogAsync);
                    CancelDialogCommand = new AnotherCommandImplementation(CancelDialog);
                    RunDialogCommand.Execute(RunDialogCommand);
                }));
            }
        }

        private void RunUpdateDialog(object obj)
        {
            DialogContent = new PlanesEdit(SelectedPlane, AirlineList);
            IsDialogOpen = true;
        }

        private async void AcceptUpdateDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            PlaneModel model = (obj as PlaneModel);
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Planes(transaction).UpdateAsync(model)))
                    {
                        transaction.Commit();
                        Message.Enqueue("Successfully updated plane \"" + model.OnboardNumber + "\"");
                    }
                    else
                    {
                        throw new Exception("Update worked incorrectly");
                    }
                }
                catch (Exception ex)
                {
                    Message.Enqueue(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    ModelConnection.SqlConnection.Instance.Close();
                    RefreshCommand.Execute(RefreshCommand);
                }
            }
            IsDialogOpen = false;
        }

        private void RunCreateDialog(object obj)
        {
            DialogContent = new PlanesEdit(AirlineList);
            IsDialogOpen = true;
        }

        private async void AcceptCreateDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            PlaneModel model = (obj as PlaneModel);
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Planes(transaction).CreateAsync(model)))
                    {
                        transaction.Commit();
                        Message.Enqueue("Successfully created airline \"" + model.OnboardNumber + "\"");
                    }
                    else
                    {
                        throw new Exception("Create worked incorrectly");
                    }
                }
                catch (Exception ex)
                {
                    Message.Enqueue(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    ModelConnection.SqlConnection.Instance.Close();
                    RefreshCommand.Execute(RefreshCommand);
                }
            }
            IsDialogOpen = false;
        }

        private void RunDeleteDialog(object obj)
        {
            DialogContent = new AcceptOrCancelDialog();
            IsDialogOpen = true;
        }

        private async void AcceptDeleteDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            PlaneModel model = SelectedPlane;
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Planes(transaction).DeleteAsync(model)))
                    {
                        transaction.Commit();
                        Message.Enqueue("Successfully delete plane \"" + model.OnboardNumber + "\"");
                    }
                    else
                    {
                        throw new Exception("Update worked incorrectly");
                    }
                }
                catch (Exception ex)
                {
                    Message.Enqueue(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    ModelConnection.SqlConnection.Instance.Close();
                    RefreshCommand.Execute(RefreshCommand);
                }
            }
            IsDialogOpen = false;
        }

        private void CancelDialog(object obj)
        {
            IsDialogOpen = false;
        }

        private RelayCommand _refreshCommand;

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand(async obj =>
                {
                    DialogContent = new ProgressDialog();
                    IsDialogOpen = true;
                    await ModelConnection.SqlConnection.Instance.OpenAsync();
                    using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
                    {
                        try
                        {
                            Planes = await Task.Run(() => ModelConnection.SqlConnection.Instance.Planes(transaction).SelectListAsync());
                            transaction.Commit();
                            Message.Enqueue("Data successfully refreshed");
                            UpdateDateTime = DateTime.Now.ToLongTimeString().ToString();
                        }
                        catch (SqlException ex)
                        {
                            Message.Enqueue(ex.Message);
                            transaction.Rollback();
                        }
                        finally
                        {
                            ModelConnection.SqlConnection.Instance.Close();
                        }
                    }
                    IsDialogOpen = false;
                }));
            }
        }
    }
}
