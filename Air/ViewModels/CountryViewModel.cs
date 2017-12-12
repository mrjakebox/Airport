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
using System.Windows;
using System.Windows.Input;

namespace Air.ViewModels
{
 public class CountryViewModel : PropertyObservable
    {
        public ObservableCollection<CountryModel> Countries
        {
            get => MainController.Instance.Countries;
            set
            {
                MainController.Instance.Countries = value;
                OnPropertyChanged("Countries");
            }
        }

        public CountryViewModel()
        {
            Message = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
        }

        private CountryModel _selectedCountry;

        public CountryModel SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
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
                return _updateCommand ?? (_updateCommand = new RelayCommand(obj =>
                {
                    if (SelectedCountry == null)
                    { Message.Enqueue("First, select an item"); return; }
                    RunDialogCommand = new AnotherCommandImplementation(RunUpdateDialog);
                    AcceptDialogCommand = new AnotherCommandImplementation(AcceptUpdateDialogAsync);
                    CancelDialogCommand = new AnotherCommandImplementation(CancelDialog);
                    RunDialogCommand.Execute(RunDialogCommand);
                }));
            }
        }

        public RelayCommand CreateCommand
        {
            get
            {
                return _createCommand ?? (_createCommand = new RelayCommand(obj =>
                {
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
                    if (SelectedCountry == null)
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
            DialogContent = new CountriesEdit(SelectedCountry);
            IsDialogOpen = true;
        }

        private async void AcceptUpdateDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            CountryModel model = (obj as CountryModel);
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Countries(transaction).UpdateAsync(model)))
                    {
                        transaction.Commit();
                        Message.Enqueue("Successfully updated country \"" + model.CountryName + "\"");
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
            DialogContent = new CountriesEdit();
            IsDialogOpen = true;
        }

        private async void AcceptCreateDialogAsync(object obj)
        {
            DialogContent = new ProgressDialog();
            CountryModel model = (obj as CountryModel);
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Countries(transaction).CreateAsync(model)))
                    {
                        transaction.Commit();
                        Message.Enqueue("Successfully created country \"" + model.CountryName + "\"");
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
            CountryModel model = SelectedCountry;
            await ModelConnection.SqlConnection.Instance.OpenAsync();
            using (SqlTransaction transaction = ((System.Data.SqlClient.SqlConnection)ModelConnection.SqlConnection.Instance.DbConnection).BeginTransaction())
            {
                try
                {
                    if (await Task.Run(() => ModelConnection.SqlConnection.Instance.Countries(transaction).DeleteAsync(model)))
                    {
                        transaction.Commit();
                        Countries.Remove(model);
                        Message.Enqueue("Successfully delete country \"" + model.CountryName + "\"");
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
                            Countries = await Task.Run(() => ModelConnection.SqlConnection.Instance.Countries(transaction).SelectListAsync());
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

        public void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("KEK");
        }
    }
}
