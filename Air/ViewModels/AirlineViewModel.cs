using Air.ModelConnection;
using MaterialDesignThemes.Wpf;
using Air.Models;
using Air.Pages.Edit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Air.ViewModels
{
    public class AirlineViewModel:PropertyObservable
    {
        public ObservableCollection<AirlineModel> Airlines
        {
            get => MainController.Instance.Airlines;
            set => MainController.Instance.Airlines = value;
        }

        public AirlineViewModel()
        {
            Message = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            AirlineModel air = new AirlineModel
            {
                AirlineID = 2,
                AirlineAddress = "wfdsa",
                AirlineName = "sdsfef",
                AirlinePhone = "11111111111"
            };
            Airlines.Add(air);
        }
        private AirlineModel _selectedAirline;

        public AirlineModel SelectedAirline
        {
            get => _selectedAirline;
            set
            {
                _selectedAirline = value;
                OnPropertyChanged("SelectedAirline");
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
                return _updateCommand ?? (_updateCommand = new RelayCommand(obj =>
                {
                    RunDialogCommand = new AnotherCommandImplementation(RunUpdateDialog);
                    AcceptDialogCommand = new AnotherCommandImplementation(AcceptUpdateDialogAsync);
                    CancelDialogCommand = new AnotherCommandImplementation(CancelDialog);
                    RunDialogCommand.Execute(RunDialogCommand);
                }));
            }
        }

        private void RunUpdateDialog(object obj)
        {
            DialogContent = new AirlinesEdit(SelectedAirline);
            IsDialogOpen = true;
        }

        private async void AcceptUpdateDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Airlines(transaction).UpdateAsync(obj as AirlineModel)))
                    {
                        transaction.Commit();
                        var airline = Airlines.First(al => al.AirlineID == (obj as AirlineModel).AirlineID);
                        airline.AirlineName = (obj as AirlineModel).AirlineName;
                        Message.Enqueue("Successfully added airline \"" + airline.AirlineName + "\"");
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
                }
            }
            IsDialogOpen = false;
        }

        private void RunCreateDialog(object obj)
        {
            DialogContent = new AirlinesEdit(new AirlineModel());
            IsDialogOpen = true;
        }

        private void AcceptCreateDialog(object obj)
        {
            DialogContent = new ProgressDialog();
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => IsDialogOpen = false, null,
                    TaskScheduler.FromCurrentSynchronizationContext());
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
                            var lines = await Task.Run(() => ModelConnection.SqlConnection.Instance.Airlines(transaction).SelectListAsync());
                            Airlines = lines as ObservableCollection<AirlineModel>;
                            transaction.Commit();
                            Message.Enqueue("Successfully refresh data");
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
