using System;
using System.Web.Mvc;

namespace calculadoracomplexa.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]  //facultativo, pq por defeito é sempre este o verbo utilizado
        public ActionResult Index() {
            //preparar os primeiros valores da calculadora
            //ou ViewBag.Visor="0" é indiferente (neste caso)
            ViewBag.Visor = 0;
            Session["operador"] = "";
            Session["limpaVisor"] = true;

            return View();
        }

        // POST: Home
        //método para responder ao post
        [HttpPost]
        //ao duplicarmos o metodo, n podemos esquecer da "chave" httpPost
        //string bt - ler o valoro do botão
        public ActionResult Index(string bt, string visor) {

            // determinar a ação a executar
            switch(bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //variavel limpaEcra
                    bool limpaEcra = (bool)Session["limpaVisor"];
                    //processa a escrita do visor
                    //se o booleano for true, ele limpa o ecra
                    //if valor no visor ou visor=="0". string é um array de caracteres
                    //Se no visor inicial é = 0, ao clicar no 0, substitui mas nao muda o q tá no visor nem limpa
                    if (limpaEcra || visor.Equals("0")) visor = bt;
                    //se não for, é substituido por outro numero cujo qual o botao foi clicado
                    else visor += bt;
                    //marcar o visor para continuar a escrita do operando
                    //é como um interroptor, se n desligar,ele continua a limpar o visor
                    Session["limpaVisor"] = false;
                    //Depois de selecionado o primeiro algarismo, se metermos zero ou outro num ele continua pra direita
                    break;

                case "+/-":
                    //
                    visor = Convert.ToDouble(visor) * -1 + ""; //qq + a string é uma string
                    break;

                case ",":
                    //Se o visor n tiver uma virgula, add
                    if (!visor.Contains(",")) visor += ",";

                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                    //Se n é a primeira vez que pressiono um operador. Se for igual a vazio, é q ele n foi selecionado
                    //Session n tem tipo, entao temos q escrever (string) antes para o definir

                    //if ((string)Session["operador"] == "")
                    if (!((string)Session["operador"]).Equals("")) {

                        //agora é q se vai fazer a 'conta'
                        //obter os operandos
                        double primeiroOperando = Convert.ToDouble((string)Session["primeiroOperando"]);

                        double segundoOperando = Convert.ToDouble(visor);

                        //escolher a operação a fazer com operador anterior
                        switch ((string)Session["operador"])
                        {
                            case "+":
                                visor = primeiroOperando + segundoOperando + ""; //meter o + "" pq se for o 1º converte pra string e só dps faz a operação
                                break;

                            case "-":
                                visor = primeiroOperando - segundoOperando + "";
                                break;
                            
                            case "x":
                                visor = primeiroOperando * segundoOperando + "";
                                break;

                            case ":":
                                visor = primeiroOperando / segundoOperando + "";
                                break;
                        } //switch((string)Session["operador"])

                        
                    } //if
                    
                    //preservar os valores fornecidos para operações futuras
                    if (bt.Equals("=")) Session["operador"] = "";
                        else Session["operador"] = bt;
                        Session["primeiroOperando"] = visor;

                        //marcar o visor para 'limpeza'
                        Session["limpaVisor"] = true;
                    break;

                case "C":
                    //vamos limpar a calculadora,
                    //isto é, fazer um "reset" total
                    visor = 0;
                    Session["operador"] = "";
                    Session["limpaVisor"] = true;

                    break;


            } //switch (bt)

            //enviar o resultado para a view
            ViewBag.Visor = visor;
            return View();
        }
    }
}