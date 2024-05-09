using System.Data; // Importa a biblioteca System.Data para usar a classe DataTable

namespace CalculadoraApp; // Define o namespace do aplicativo como CalculadoraApp

public partial class Calculadora : ContentPage // Define a classe Calculadora que herda de ContentPage
{
	public Calculadora() // Construtor da classe Calculadora
	{
		InitializeComponent(); // Inicializa os componentes da interface gráfica
	}

	int contador = 0; // Inicializa um contador com valor 0

	private void calc_Clicked(object sender, EventArgs e) // Método chamado ao clicar em um botão numérico ou de operação
	{
		Button b = (Button)sender; // Obtém o botão que foi clicado
		float f = float.TryParse(b.Text, out float res) ? res : 0; // Tenta converter o texto do botão para um float, se não conseguir, define como 0

		if (f != 0) // Se o valor convertido for um número
		{
			lblResultado.Text = lblResultado.Text == "0" ? f.ToString() : lblResultado.Text + f.ToString(); // Atualiza o label do resultado com o número clicado
			if (contador >= 1) result_Clicked(); // Se já houver uma operação em andamento, calcula o resultado
		}
		else // Se o valor convertido não for um número (operador)
		{
			if (lblResultado.Text.Substring(lblResultado.Text.Length - 1) == "+" || // Verifica se o último caractere é um operador
				lblResultado.Text.Substring(lblResultado.Text.Length - 1) == "×" ||
				lblResultado.Text.Substring(lblResultado.Text.Length - 1) == "−" ||
				lblResultado.Text.Substring(lblResultado.Text.Length - 1) == "÷")
			{
				lblResultado.Text = lblResultado.Text.Substring(0, lblResultado.Text.Length - 1) + b.Text; // Substitui o operador anterior pelo novo operador
			}
			else
			{
				lblResultado.Text += b.Text; // Adiciona o operador ao label do resultado
				contador++; // Incrementa o contador de operações
			}
		}
	}

	private void result_Clicked() // Método chamado ao clicar no botão de igual (=)
	{
		var calcular = lblResultado.Text.Replace("×", "*").Replace("÷", "/").Replace("–", "-"); // Substitui os operadores por símbolos compreendidos pelo DataTable
		double resultado = Convert.ToDouble(new DataTable().Compute(calcular, null)); // Calcula o resultado usando o DataTable

		lblHistorico.Text = resultado.ToString(); // Exibe o resultado no label do histórico
	}

	private void btnIgual_Clicked(object sender, EventArgs e) // Método chamado ao clicar no botão de "ans"
	{
		if (lblHistorico.Text != "") // Verifica se há um resultado no histórico
		{
			lblResultado.Text = lblHistorico.Text; // Exibe o resultado no label do resultado
			lblHistorico.Text = ""; // Limpa o histórico
		}
	}

	private void btnCancel_Clicked(object sender, EventArgs e) // Método chamado ao clicar em um botão de limpar (AC, C, ou backspace)
	{
		Button b = (Button)sender; // Obtém o botão que foi clicado

		if (b == btnAC || b == btnC) // Se o botão for AC ou C
		{
			lblHistorico.Text = "0"; // Limpa o histórico
			lblResultado.Text = "0"; // Limpa o resultado
		}
		else if (b == btnC) // Se o botão for C
		{
			lblResultado.Text = "0"; // Limpa o resultado
		}
		else // Se o botão for backspace
		{
			if (lblResultado.Text != "0") // Verifica se o resultado não é zero
			{
				lblResultado.Text = lblResultado.Text.Remove(lblResultado.Text.Length - 1); // Remove o último caractere do resultado
				if (string.IsNullOrEmpty(lblResultado.Text)) // Verifica se o resultado ficou vazio
				{
					lblResultado.Text = "0"; // Define o resultado como zero
				}
			}
		}
	}

	private void btnDecimal_Clicked(object sender, EventArgs e) // Método chamado ao clicar no botão de ponto decimal (.)
	{
		if (lblResultado.Text.Contains(".") is false) // Verifica se o resultado já possui um ponto decimal
		{
			lblResultado.Text += "."; // Adiciona o ponto decimal ao resultado
		}
	}
}