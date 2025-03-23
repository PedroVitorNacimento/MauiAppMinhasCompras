// Importa os modelos de dados do aplicativo
using MauiAppMinhasCompras.Models;
// Importa a classe ObservableCollection para armazenar a lista de produtos e permitir atualiza��es autom�ticas na interface
using System.Collections.ObjectModel;

// Define o namespace onde esta classe est� localizada
namespace MauiAppMinhasCompras.Views;

// Define a classe ListaProduto que representa a tela de listagem de produtos
public partial class ListaProduto : ContentPage
{
    // Declara uma cole��o observ�vel que armazena os produtos exibidos na lista
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    // Construtor da classe, executado ao criar a tela
    public ListaProduto()
    {
        InitializeComponent(); // Inicializa os componentes visuais definidos no XAML

        // Define a cole��o "lista" como a fonte de dados da ListView "lst_produtos"
        lst_produtos.ItemsSource = lista;
    }

    // M�todo que � chamado quando a tela aparece
    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            // Obt�m todos os produtos do banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona os produtos na cole��o observ�vel para exibi��o na interface
            tmp.ForEach(i => lista.Add(i));
        }

        catch (Exception ex) {
            // Exibe um alerta caso ocorra algum erro
            await DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usu�rio clica no bot�o "Adicionar"
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Navega para a tela de cadastro de um novo produto
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            // Exibe um alerta caso ocorra algum erro
           await DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o texto no campo de pesquisa � alterado
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // Obt�m o texto digitado pelo usu�rio
            string q = e.NewTextValue;

            // Limpa a lista de produtos antes de adicionar os resultados da pesquisa
            lista.Clear();

            // Realiza a pesquisa no banco de dados com base no texto digitado
            List<Produto> tmp = await App.Db.search(q);

            // Adiciona os produtos encontrados � lista observ�vel
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Exibe um alerta caso ocorra algum erro
            await DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usu�rio clica no bot�o "Somar"
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Calcula a soma total dos valores dos produtos na lista
        double soma = lista.Sum(i => i.Total);

        // Cria uma mensagem formatada com o total calculado
        string msg = $"O total � {soma:c}";

        // Exibe um alerta mostrando o valor total dos produtos
        DisplayAlert("Total dos Produtos", msg, "ok");
    }

    // Vari�vel para armazenar o item selecionado pelo usu�rio
    private Produto? itemSelecionado;

    // Evento chamado quando um item da lista � selecionado

/* Altera��o n�o mesclada do projeto 'MauiAppMinhasCompras (net8.0-ios)'
Antes:
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
Ap�s:
    private async Task lst_produtos_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
    {
*/
    private  void lst_produtos_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Armazena o produto selecionado na vari�vel "itemSelecionado"
            itemSelecionado = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto { BindingContext = itemSelecionado });
        }
        catch (Exception ex) 
        {
            // Exibe um alerta caso ocorra algum erro
             DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usu�rio clica no bot�o "Remover"
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        // Verifica se algum item foi selecionado antes de tentar remover
        if (itemSelecionado == null)
        {
            // Exibe um alerta informando que � necess�rio selecionar um item
            await DisplayAlert("Aviso", "Selecione um item para remover.", "OK");
            return;
        }

        // Exibe um alerta de confirma��o antes de remover o item
        bool confirm = await DisplayAlert("Confirma��o", $"Deseja remover {itemSelecionado.Descricao}?", "Sim", "N�o");

        // Se o usu�rio confirmar a remo��o
        if (confirm)
        {
            // Remove o item do banco de dados
            await App.Db.Delete(itemSelecionado.Id);

            // Remove o item da cole��o observ�vel (para atualizar a interface)
            lista.Remove(itemSelecionado);

            // Reseta a vari�vel "itemSelecionado" para evitar problemas futuros
            itemSelecionado = null;
        }
    }
}
