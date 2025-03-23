// Importa os modelos de dados do aplicativo
using MauiAppMinhasCompras.Models;
// Importa a classe ObservableCollection para armazenar a lista de produtos e permitir atualizações automáticas na interface
using System.Collections.ObjectModel;

// Define o namespace onde esta classe está localizada
namespace MauiAppMinhasCompras.Views;

// Define a classe ListaProduto que representa a tela de listagem de produtos
public partial class ListaProduto : ContentPage
{
    // Declara uma coleção observável que armazena os produtos exibidos na lista
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    // Construtor da classe, executado ao criar a tela
    public ListaProduto()
    {
        InitializeComponent(); // Inicializa os componentes visuais definidos no XAML

        // Define a coleção "lista" como a fonte de dados da ListView "lst_produtos"
        lst_produtos.ItemsSource = lista;
    }

    // Método que é chamado quando a tela aparece
    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            // Obtém todos os produtos do banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona os produtos na coleção observável para exibição na interface
            tmp.ForEach(i => lista.Add(i));
        }

        catch (Exception ex) {
            // Exibe um alerta caso ocorra algum erro
            await DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usuário clica no botão "Adicionar"
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

    // Evento chamado quando o texto no campo de pesquisa é alterado
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // Obtém o texto digitado pelo usuário
            string q = e.NewTextValue;

            // Limpa a lista de produtos antes de adicionar os resultados da pesquisa
            lista.Clear();

            // Realiza a pesquisa no banco de dados com base no texto digitado
            List<Produto> tmp = await App.Db.search(q);

            // Adiciona os produtos encontrados à lista observável
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Exibe um alerta caso ocorra algum erro
            await DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usuário clica no botão "Somar"
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Calcula a soma total dos valores dos produtos na lista
        double soma = lista.Sum(i => i.Total);

        // Cria uma mensagem formatada com o total calculado
        string msg = $"O total é {soma:c}";

        // Exibe um alerta mostrando o valor total dos produtos
        DisplayAlert("Total dos Produtos", msg, "ok");
    }

    // Variável para armazenar o item selecionado pelo usuário
    private Produto? itemSelecionado;

    // Evento chamado quando um item da lista é selecionado

/* Alteração não mesclada do projeto 'MauiAppMinhasCompras (net8.0-ios)'
Antes:
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
Após:
    private async Task lst_produtos_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
    {
*/
    private  void lst_produtos_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Armazena o produto selecionado na variável "itemSelecionado"
            itemSelecionado = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto { BindingContext = itemSelecionado });
        }
        catch (Exception ex) 
        {
            // Exibe um alerta caso ocorra algum erro
             DisplayAlert("Ops", ex.Message, "ok");
        }
    }

    // Evento chamado quando o usuário clica no botão "Remover"
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        // Verifica se algum item foi selecionado antes de tentar remover
        if (itemSelecionado == null)
        {
            // Exibe um alerta informando que é necessário selecionar um item
            await DisplayAlert("Aviso", "Selecione um item para remover.", "OK");
            return;
        }

        // Exibe um alerta de confirmação antes de remover o item
        bool confirm = await DisplayAlert("Confirmação", $"Deseja remover {itemSelecionado.Descricao}?", "Sim", "Não");

        // Se o usuário confirmar a remoção
        if (confirm)
        {
            // Remove o item do banco de dados
            await App.Db.Delete(itemSelecionado.Id);

            // Remove o item da coleção observável (para atualizar a interface)
            lista.Remove(itemSelecionado);

            // Reseta a variável "itemSelecionado" para evitar problemas futuros
            itemSelecionado = null;
        }
    }
}
