using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gramatica.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        [HttpPost]
        public ActionResult creaDiccionario(string gramatica)
        {
            Dictionary<string, List<string>> producciones = new Dictionary<string, List<string>>();
            List<string> value = new List<string>();
            string[] partes;
            string key = "";


            partes = gramatica.Split('\n');

            for (int i = 0; i < partes.Length; i++)
            {

                string temp = partes[i];
                string temp2 = "";
                string[] key2temp = temp.Split('-');
                key = temp[0].ToString();
                if (/*key.Length == 1*/key2temp[0].Length == 1 && IsAllUpper(key))
                {
                    if (temp.Contains("&"))
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {
                            if (temp[j].Equals('&'))
                                temp2 += "Ɛ";
                            else
                                temp2 += temp[j];
                        }
                        temp = "";
                        temp = temp2;
                    }

                    string[] valores = temp.Split('>');

                    string[] valorIndividual = valores[1].Split('|');


                    if (producciones.ContainsKey(key))
                    {
                        for (int k = 0; k < producciones[key].Count; k++)
                            value.Add(producciones[key][k]);

                        for (int k = 0; k < valorIndividual.Length; k++)
                            if (!value.Contains(valorIndividual[k]))
                                value.Add(valorIndividual[k]);

                        producciones[key] = value;

                        value = new List<string>();
                    }
                    else
                    {
                        for (int j = 0; j < valorIndividual.Length; j++)
                            value.Add(valorIndividual[j]);
                        producciones.Add(key, value);
                        value = new List<string>();
                    }
                }
                else
                {
                    producciones = new Dictionary<string, List<string>>();
                    producciones.Add("X", new List<string> { "ERROR" });
                }
            }

            return View("imprimeGramatica", producciones);
        }


        [HttpPost]
        public ActionResult creaPrimeros(string gramatica)
        {
            Dictionary<string, List<string>> producciones = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            List<string> value = new List<string>();
            string[] partes;
            string key = "";


            partes = gramatica.Split('\n');

            for (int i = 0; i < partes.Length; i++)
            {

                string temp = partes[i];
                string temp2 = "";
                string[] key2temp = temp.Split('-');
                key = temp[0].ToString();
                if (/*key.Length == 1*/key2temp[0].Length == 1 && IsAllUpper(key))
                {
                    if (temp.Contains("&"))
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {
                            if (temp[j].Equals('&'))
                                temp2 += "Ɛ";
                            else
                                temp2 += temp[j];
                        }
                        temp = "";
                        temp = temp2;
                    }

                    string[] valores = temp.Split('>');

                    string[] valorIndividual = valores[1].Split('|');


                    if (producciones.ContainsKey(key))
                    {
                        for (int k = 0; k < producciones[key].Count; k++)
                            value.Add(producciones[key][k]);

                        for (int k = 0; k < valorIndividual.Length; k++)
                            if (!value.Contains(valorIndividual[k]))
                                value.Add(valorIndividual[k]);

                        producciones[key] = value;

                        value = new List<string>();
                    }
                    else
                    {
                        for (int j = 0; j < valorIndividual.Length; j++)
                            value.Add(valorIndividual[j]);
                        producciones.Add(key, value);
                        value = new List<string>();
                    }

                }
                else
                {
                    producciones = new Dictionary<string, List<string>>();
                    producciones.Add("X", new List<string> { "ERROR" });
                }
            }


            primeros = Primeros(producciones);


            return View("imprimeGramatica", primeros);
        }


        private Dictionary<string, List<string>> Primeros(Dictionary<string, List<string>> produc)
        {
            List<string> llaves = new List<string>(produc.Keys);
            List<string> primerosLista = new List<string>();
            List<string> obtenidos = new List<string>();
            List<string> primerosRec = new List<string>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> prod = new Dictionary<string, List<string>>();
            List<string> produccionRecLista = new List<string>();
            //Dictionary<string, List<string>> recursivo = new Dictionary<string, List<string>>();
            List<string> Recursivas = new List<string>();
            prod = ordenaDiccionario(produc);

            int k;
            bool continuar;
            for (int i = 0; i < llaves.Count; i++)//recorre cade llave
            {
                for (int j = 0; j < prod[llaves[i]].Count; j++)//Recorre cada produccion de la llave
                {
                    k = 0; continuar = true;
                    while (continuar == true && k < prod[llaves[i]][j].Length /*int k = 0; k < prod[llaves[i]][j].Length; k++*/)//Recorre cada caracter de la produccion
                    {
                        if (IsAllUpper(prod[llaves[i]][j][k].ToString()))//SI EL caracter es un no terminal
                        {
                            if (llaves[i].Equals(prod[llaves[i]][j][k].ToString()))
                            {
                                //llaveRecursivas.Ad(llaves[i]);       //SI SE HACE AQUI                         
                                primerosRec = Recursivo(prod, llaves[i]);
                                //
                                for (int l = 0; l < primerosRec.Count; l++)
                                {
                                    if (!primerosRec[l].Equals("Ɛ"))
                                        if (!primerosLista.Contains(primerosRec[l]))
                                            primerosLista.Add(primerosRec[l]);
                                }
                                continuar = false;
                            }
                            else
                            {
                                obtenidos = ObtenPrimero(prod, prod[llaves[i]][j][k].ToString());
                                for (int l = 0; l < obtenidos.Count; l++)
                                {
                                    if (!obtenidos[l].Equals("Ɛ"))
                                        if (!primerosLista.Contains(obtenidos[l]))
                                            primerosLista.Add(obtenidos[l]);
                                }
                                if (!obtenidos.Contains("Ɛ"))
                                    continuar = false;
                            }

                        }
                        else///SI EL CARACTER ES UN TERMINAL 
                        {
                            primerosLista.Add(prod[llaves[i]][j][k].ToString());
                            continuar = false;
                        }
                        k++;
                    }
                    if (continuar)
                        primerosLista.Add("Ɛ");
                }
                primeros.Add(llaves[i], primerosLista);
                primerosLista = new List<string>();

            }
            //Recursivo(prod,);//mandar el diccionario completo y la llave 

            //agregar los no terminales al diccionario de primeros
            primeros = AgregaTerminalesAPrimeros(primeros, produc);
            return primeros;

        }

        private List<string> ObtenPrimero(Dictionary<string, List<string>> prod, string llav)//no chea si es recursivo
        {
            int k;
            bool continuar;
            List<string> recuperados = new List<string>();
            List<string> obtenidos = new List<string>();
            List<string> produccionRecLista = new List<string>();
            string produccionRec = "";
            for (int i = 0; i < prod[llav].Count; i++)//Reccorre las producciones de la llave 
            {
                k = 0; continuar = true;
                while (continuar == true && k < prod[llav][i].Length)//Recorre cada caracter de la produccion
                {
                    if (IsAllUpper(prod[llav][i][k].ToString()))//SI EL caracter es un no terminal
                    {

                        if (llav.Equals(prod[llav][i][k].ToString()))///Si la llave y el caracter  son el mismo no terminal
                        {
                            produccionRec = prod[llav][i].ToString();
                            produccionRecLista.Add(produccionRec);
                            continuar = false;
                        }
                        else
                        {
                            obtenidos = ObtenPrimero(prod, prod[llav][i][k].ToString());
                            for (int l = 0; l < obtenidos.Count; l++)
                            {
                                if (!obtenidos[l].Equals("Ɛ"))
                                    recuperados.Add(obtenidos[l]);
                            }
                            if (!obtenidos.Contains("Ɛ"))
                                continuar = false;
                        }
                    }
                    else///SI EL CARACTER ES UN TERMINAL 
                    {
                        recuperados.Add(prod[llav][i][k].ToString());
                        if (!prod[llav][i][k].ToString().Equals("Ɛ"))
                            continuar = false;
                        //meter en una lista

                    }
                    k++;
                }
                if (continuar && !recuperados.Contains("Ɛ"))
                {
                    recuperados.Add("Ɛ");
                    for (int a = 0; a < produccionRecLista.Count; a++)
                    {
                        for (int r = 0; r < produccionRecLista[a].Length; r++/*int r = 0; r < produccionRec.Length; r++*/)
                        {
                            if (produccionRecLista[a][r].ToString().Equals(llav.ToString())/*produccionRec[r].ToString().Equals(llav.ToString())*/)
                            {
                                if (/*produccionRec.Length > 1*/produccionRecLista[a].Length > 1)
                                {
                                    if (!recuperados.Contains(produccionRecLista[a][r + 1].ToString())/*!recuperados.Contains(produccionRec[r + 1].ToString())*/)
                                    {
                                        //recuperados.Add(produccionRec[r + 1].ToString());
                                        recuperados.Add(produccionRecLista[a][r + 1].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return recuperados;
        }

        private List<string> Recursivo(Dictionary<string, List<string>> prod, string llav)
        {
            string produccionRec = "";
            List<string> llaves = new List<string>(prod.Keys);
            List<string> primerosLista = new List<string>();
            List<string> obtenidos = new List<string>();
            List<string> produccionRecursivo = new List<string>();
            List<string> produccionRecLista = new List<string>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            //Dictionary<string, List<string>> recursivo = new Dictionary<string, List<string>>();

            int k;
            bool continuar;
            for (int i = 0; i < llaves.Count; i++)//recorre cade llave
            {
                if (llaves[i].Equals(llav))
                {
                    for (int j = 0; j < prod[llaves[i]].Count; j++)//Recorre cada produccion de la llave
                    {
                        k = 0; continuar = true;
                        while (continuar == true && k < prod[llaves[i]][j].Length /*int k = 0; k < prod[llaves[i]][j].Length; k++*/)//Recorre cada caracter de la produccion
                        {
                            if (IsAllUpper(prod[llaves[i]][j][k].ToString()))//SI EL caracter es un no terminal
                            {
                                if (llaves[i].Equals(prod[llaves[i]][j][k].ToString()))///Si la llave y el caracter  son el mismo no terminal
                                {
                                    //recursivo.Add(llaves[i], prod[llaves[i]]);
                                    produccionRec = prod[llaves[i]][j].ToString();
                                    produccionRecLista.Add(produccionRec); //------------------------------------------
                                    continuar = false;
                                }
                                else
                                {
                                    obtenidos = ObtenPrimero(prod, prod[llaves[i]][j][k].ToString());
                                    for (int l = 0; l < obtenidos.Count; l++)
                                    {
                                        if (!obtenidos[l].Equals("Ɛ"))
                                            if (!primerosLista.Contains(obtenidos[l]))
                                                primerosLista.Add(obtenidos[l]);
                                    }
                                    if (!obtenidos.Contains("Ɛ"))
                                        continuar = false;
                                }

                            }
                            else///SI EL CARACTER ES UN TERMINAL 
                            {
                                primerosLista.Add(prod[llaves[i]][j][k].ToString());
                                continuar = false;
                            }
                            k++;
                        }
                        if (continuar)
                        {
                            primerosLista.Add("Ɛ");
                            for (int a = 0; a < produccionRecLista.Count; a++)
                            {
                                for (int r = 0; r < produccionRecLista[a].Length; r++/*int r = 0; r < produccionRec.Length; r++*/)
                                {
                                    if (produccionRecLista[a][r].ToString().Equals(llaves[i].ToString())/*produccionRec[r].ToString().Equals(llaves[i].ToString())*/)
                                    {
                                        if (produccionRecLista[a].Length > 1/*produccionRec.Length > 1*/)
                                        {
                                            if (!primerosLista.Contains(produccionRecLista[a][r + 1].ToString())/*!primerosLista.Contains(produccionRec[r + 1].ToString())*/)
                                            {
                                                //primerosLista.Add(produccionRec[r + 1].ToString());
                                                primerosLista.Add(produccionRecLista[a][r + 1].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    primeros.Add(llaves[i], primerosLista);
                    //return primeros;;
                    //primerosLista = new List<string>();
                }

            }
            return primerosLista;
        }

        bool IsAllUpper(string input)
        {
            if (input.Equals("Ɛ"))
                return false;

            if (char.IsUpper(input[0]))
                return true;
            else
                return false;
            /*for (int i = 0; i < input.Length; i++)
            {                
                    if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                        return false;                                                                    
            }
            if (input.Equals("Ɛ"))
                return false;
            return true;*/
        }

        private Dictionary<string, List<string>> ordenaDiccionario(Dictionary<string, List<string>> prod)
        {
            Dictionary<string, List<string>> produccionesOrdenadas = new Dictionary<string, List<string>>();
            List<string> llaves = new List<string>(prod.Keys);


            for (int i = 0; i < prod.Count; i++)
            {
                List<string> recursivas = new List<string>();
                List<string> noRecursivas = new List<string>();
                List<string> todasProducciones = new List<string>();
                for (int j = 0; j < prod[llaves[i]].Count; j++)
                {
                    if (prod[llaves[i]][j][0].ToString().Equals(llaves[i].ToString()))
                        recursivas.Add(prod[llaves[i]][j].ToString());
                    else
                        noRecursivas.Add(prod[llaves[i]][j].ToString());
                }
                for (int j = 0; j < recursivas.Count; j++)
                    todasProducciones.Add(recursivas[j]);

                for (int j = 0; j < noRecursivas.Count; j++)
                    todasProducciones.Add(noRecursivas[j]);

                produccionesOrdenadas.Add(llaves[i], todasProducciones);

                /*List<string> recursivas = new List<string>();
                List<string> noRecursivas = new List<string>();
                List<string> todasProducciones = new List<string>();*/

            }


            return produccionesOrdenadas;
        }

        private Dictionary<string, List<string>> AgregaTerminalesAPrimeros(Dictionary<string, List<string>> primeros, Dictionary<string, List<string>> produc)
        {
            List<string> llaves = new List<string>(produc.Keys);
            List<string> listaTerminales = new List<string>();
            List<string> valorTerminal;
            for (int i = 0; i < llaves.Count; i++)//itera las llaves
                for (int j = 0; j < produc[llaves[i]].Count; j++)//itera la lista de la llave
                    for (int k = 0; k < produc[llaves[i]][j].Length; k++)//itera cada cadena de la lista
                        if (!IsAllUpper(produc[llaves[i]][j][k].ToString()) && !produc[llaves[i]][j][k].ToString().Equals("Ɛ") && !listaTerminales.Contains(produc[llaves[i]][j][k].ToString()))
                            listaTerminales.Add(produc[llaves[i]][j][k].ToString());

            for (int i = 0; i < listaTerminales.Count; i++)
            {
                valorTerminal = new List<string>();
                valorTerminal.Add(listaTerminales[i]);
                primeros.Add(listaTerminales[i], valorTerminal);
            }
            return primeros;
        }


        [HttpPost]
        public ActionResult ObtenerSiguientes(string gramatica)
        {
            Dictionary<string, List<string>> producciones = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> siguientes = new Dictionary<string, List<string>>();
            List<string> value = new List<string>();
            string[] partes;
            string key = "";


            partes = gramatica.Split('\n');

            for (int i = 0; i < partes.Length; i++)
            {

                string temp = partes[i];
                string temp2 = "";
                string[] key2temp = temp.Split('-');
                key = temp[0].ToString();
                if (/*key.Length == 1*/key2temp[0].Length == 1 && IsAllUpper(key))
                {
                    if (temp.Contains("&"))
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {
                            if (temp[j].Equals('&'))
                                temp2 += "Ɛ";
                            else
                                temp2 += temp[j];
                        }
                        temp = "";
                        temp = temp2;
                    }

                    string[] valores = temp.Split('>');

                    string[] valorIndividual = valores[1].Split('|');


                    if (producciones.ContainsKey(key))
                    {
                        for (int k = 0; k < producciones[key].Count; k++)
                            value.Add(producciones[key][k]);

                        for (int k = 0; k < valorIndividual.Length; k++)
                            if (!value.Contains(valorIndividual[k]))
                                value.Add(valorIndividual[k]);

                        producciones[key] = value;

                        value = new List<string>();
                    }
                    else
                    {
                        for (int j = 0; j < valorIndividual.Length; j++)
                            value.Add(valorIndividual[j]);
                        producciones.Add(key, value);
                        value = new List<string>();
                    }

                }
                else
                {
                    producciones = new Dictionary<string, List<string>>();
                    producciones.Add("X", new List<string> { "ERROR" });
                }
            }


            primeros = Primeros(producciones);
            siguientes = Siguientes(primeros, producciones);



            return View("imprimeGramatica", siguientes);
        }


        private Dictionary<string, List<string>> Siguientes(Dictionary<string, List<string>> primeros, Dictionary<string, List<string>> produc)
        {
            List<string> llavesProducciones = new List<string>(produc.Keys);
            List<string> aparece = new List<string>();
            Dictionary<string, List<string>> siguientes = new Dictionary<string, List<string>>();
            List<List<string>> tiposDeProd = new List<List<string>>();

            //List<string> llavesPrimeros = new List<string>(primeros.Keys);

            for (int i = 0; i < llavesProducciones.Count; i++)//Itera las llaves de las producciones
            {
                List<string> siguiente = new List<string>();
                if (i == 0)
                    siguiente.Add("$");
                aparece = ObtenerProduccionesDondeAparesca(produc, llavesProducciones, llavesProducciones[i]);//obtiene las producciones donde aparece la llave
                tiposDeProd = TipoDeProduccion(aparece, llavesProducciones[i]);


                for (int j = 0; j < tiposDeProd.Count; j++)//Itera los dos tipos de produccion
                {
                    //for(int k = 0; k < tiposDeProd[j].Count;k++)//itera las cadenas del tipo de produccion
                    //{
                    if (j == 0)//se van a obtener los primeros de beta en alfa B beta
                    {
                        produccionTipoABB(tiposDeProd[0], llavesProducciones[i], primeros, siguiente, siguientes, produc);
                    }
                    else if (j == 1)//se van a obtener los siguientes de A en  aB
                    {
                        produccionTipoAB(tiposDeProd[1], llavesProducciones[i], siguiente, siguientes);
                    }
                    //}
                }

                if (i == 0 && !siguientes.ContainsKey(llavesProducciones[i]))
                    siguientes.Add(llavesProducciones[i], siguiente);
            }

            return siguientes;
        }


        private List<string> ObtenerProduccionesDondeAparesca(Dictionary<string, List<string>> producciones, List<string> claves, string llave)
        {
            List<string> produccionesQueContienenLaLlave = new List<string>();
            for (int i = 0; i < claves.Count; i++)//Itera las llaves
                for (int j = 0; j < producciones[claves[i]].Count; j++)//itera la lista de la llave
                    if (producciones[claves[i]][j].Contains(llave))
                        produccionesQueContienenLaLlave.Add(claves[i] + "->" + producciones[claves[i]][j]);

            return produccionesQueContienenLaLlave;
        }

        private List<List<string>> TipoDeProduccion(List<string> aparecen, string clave)
        {
            List<List<string>> tipoProdu = new List<List<string>>();
            List<string> ABB = new List<string>();
            List<string> AB = new List<string>();

            for (int i = 0; i < aparecen.Count; i++)
            {
                if (aparecen[i][aparecen[i].Length - 1].ToString().Equals(clave.ToString()) && aparecen[i].Length > 3)
                    AB.Add(aparecen[i]);
                else
                    ABB.Add(aparecen[i]);
            }
            tipoProdu.Add(ABB);
            tipoProdu.Add(AB);
            return tipoProdu;
        }

        private void produccionTipoABB(List<string> tiposPABB, string clave, Dictionary<string, List<string>> primerosP, List<string> sig, Dictionary<string, List<string>> siguientesP, Dictionary<string, List<string>> producc)
        {

            for (int i = 0; i < tiposPABB.Count; i++)//itera las proudcciones de tipo ABB
            {
                Dictionary<string, List<string>> prodAux = new Dictionary<string, List<string>>();
                string[] produccionSeparada = tiposPABB[i].Split('>');
                int index = produccionSeparada[1].IndexOf(clave);
                string beta = "";
                //List<string> primerosDeBeta = new List<string>(primerosP[produccionSeparada[1][index+1].ToString()]);
                for (int j = index + 1; j < produccionSeparada[1].Length; j++)
                    beta += produccionSeparada[1][j];
                prodAux = copiaDiccionario(producc, clave, produccionSeparada[0][0].ToString(), beta);

                List<string> primerosDeBeta = ObtenPrimero(prodAux, produccionSeparada[0][0].ToString());


                List<string> sigAux = new List<string>();
                for (int l = 0; l < primerosDeBeta.Count; l++)
                {
                    if (!primerosDeBeta[l].Equals("Ɛ") && !sig.Contains(primerosDeBeta[l]))
                    {
                        sig.Add(primerosDeBeta[l]);
                    }
                }
                if (primerosDeBeta.Contains("Ɛ"))//si el primero de beta contiene epsilon
                {
                    if (!produccionSeparada[0][0].ToString().Equals(produccionSeparada[1][index + 1].ToString()))//si el encabezado es diferente de beta
                    {
                        if (siguientesP.ContainsKey(produccionSeparada[0][0].ToString()))// si ya se tiene calculado A en siguientes
                        {
                            List<string> siguientesDeA = new List<string>(siguientesP[produccionSeparada[0][0].ToString()]);
                            for (int j = 0; j < siguientesDeA.Count; j++)
                                if (!sig.Contains(siguientesDeA[j]))
                                    sig.Add(siguientesDeA[j]);
                        }
                        else//SI NO SE HAN CALCULADO LO SIGUIENTES DE A
                        {
                            //-------------------------------------------------------------------------
                            //-------------------------------------------------------------------------
                            //-------------------------------------------------------------------------
                        }
                    }
                    else { }//si el encabezado es igual a beta no se hace nada
                }
                if (siguientesP.ContainsKey(clave))
                {
                    sigAux = siguientesP[clave];
                    for (int j = 0; j < sig.Count; j++)
                        if (!sigAux.Contains(sig[j]))
                            sigAux.Add(sig[j]);
                    //siguientesP.Add(clave, sig);
                    siguientesP[clave] = sigAux;
                }
                else
                    siguientesP.Add(clave, sig);
            }
            /*if (primerosDeBeta.Contains("Ɛ"))// si el primero de Beta contiene Epsilon
            else // si el primero de Beta no contiene epsilon
                siguientes.Add(llavesProducciones[i], siguiente);// si no lo contiene agrega los primeros capturados al diccionario sde sigueintes                            */
        }

        private void produccionTipoAB(List<string> tiposPAB, string clave, List<string> sig, Dictionary<string, List<string>> siguientesP)
        {
            List<string> sigAux = new List<string>();
            for (int i = 0; i < tiposPAB.Count; i++)
            {
                string[] produccionSeparada = tiposPAB[i].Split('>');
                int index = produccionSeparada[1].IndexOf(clave);

                if (!produccionSeparada[0][0].ToString().Equals(produccionSeparada[1][index].ToString()))//si el encabezado es diferente de B
                {
                    if (siguientesP.ContainsKey(produccionSeparada[0][0].ToString()))// si ya se tiene calculado A en siguientes
                    {
                        List<string> siguientesDeA = new List<string>(siguientesP[produccionSeparada[0][0].ToString()]);
                        for (int j = 0; j < siguientesDeA.Count; j++)
                            if (!sig.Contains(siguientesDeA[j]))
                                sig.Add(siguientesDeA[j]);
                    }
                    else//SI NO SE HAN CALCULADO LO SIGUIENTES DE A
                    {
                        //-------------------------------------------------------------------------
                        //-------------------------------------------------------------------------
                        //-------------------------------------------------------------------------
                    }
                }
                else { }//si el encabezado es igual a B no se hace nada

                if (siguientesP.ContainsKey(clave))
                {
                    sigAux = siguientesP[clave];
                    for (int j = 0; j < sig.Count; j++)
                        if (!sigAux.Contains(sig[j]))
                            sigAux.Add(sig[j]);
                    //siguientesP.Add(clave, sig);
                    siguientesP[clave] = sigAux;
                }
                else
                    siguientesP.Add(clave, sig);
            }
        }

        private Dictionary<string, List<string>> copiaDiccionario(Dictionary<string, List<string>> prodOriginal, string cl, string encabezado, string nuevaProd)
        {
            List<string> nP = new List<string>();
            Dictionary<string, List<string>> prodNuevo = new Dictionary<string, List<string>>(prodOriginal);
            List<string> llaves = new List<string>(prodOriginal.Keys);

            /*for (int i = 0; i < llaves.Count;i++)
            {
                prodNuevo.Add(llaves[i], prodOriginal[llaves[i]]);
            }*/
            //Dictionary<string, List<string>> prodNuevo = prodOriginal;

            prodNuevo.Remove(encabezado);

            nP.Add(nuevaProd);
            prodNuevo.Add(encabezado, nP);

            return prodNuevo;
        }

        [HttpPost]
        public ActionResult AnalizadorSintacticoLR(string cadena)
        {
            Dictionary<string, List<string>> gramatica = new Dictionary<string, List<string>>();
            List<int> pila = new List<int>();
            string[,] tablaLR = new string[10, 6];
            string cadenaOriginal = cadena;
            string cadenaAccion = "";
            int s = 0;
            int t = 0;
            int indiceA = 0;
            string a = "";
            string resultado = "";
            bool salida = false;
            cadena += "$";
            pila.Add(0);
            a = cadena[indiceA].ToString();
            tablaLR = LlenaTablaLR();

            gramatica.Add("P", new List<string> { "aPa", "bPb", "c" });
            while (salida != true)
            {
                s = pila[pila.Count - 1]; // s apunta al tope de la pila
                cadenaAccion = Accion(s, a, tablaLR);
                if (cadenaAccion[0].ToString().Equals("d".ToString()))
                {
                    int.TryParse(cadenaAccion[1].ToString(), out t);
                    pila.Add(t);
                    indiceA++;
                    a = cadena[indiceA].ToString();
                }
                else if (cadenaAccion[0].ToString().Equals("r".ToString()))
                {
                    int numProd, prodLength;
                    int.TryParse(cadenaAccion[1].ToString(), out numProd);
                    int irA;
                    prodLength = gramatica["P"][numProd - 1].Length;
                    pila = pop(prodLength, pila);
                    t = pila[pila.Count - 1];
                    int.TryParse(Accion(t, "P", tablaLR), out irA);
                    pila.Add(irA);
                }
                else if (cadenaAccion.ToString().Equals("AC".ToString()))
                {
                    resultado = "La cadena '" + cadenaOriginal + "' si pertenece al lenguaje de la gramática";
                    salida = true;
                }
                else
                {
                    resultado = "La cadena '" + cadenaOriginal + "' no pertenece al lenguaje de la gramática";
                    salida = true;
                }
            }
            List<string> res = new List<string>();
            res.Add(resultado);
            return View("AnalizadorSintactico", res);
        }

        private string[,] LlenaTablaLR()
        {
            string[,] tabla = new string[10, 6];
            //Primer Renglón
            tabla[0, 0] = "error";
            tabla[0, 1] = "a";
            tabla[0, 2] = "b";
            tabla[0, 3] = "c";
            tabla[0, 4] = "$";
            tabla[0, 5] = "P";

            //Segundo Renglón
            tabla[1, 0] = "0";
            tabla[1, 1] = "d1";
            tabla[1, 2] = "d2";
            tabla[1, 3] = "d3";
            tabla[1, 4] = "error";
            tabla[1, 5] = "4";

            //Tercer Renglón
            tabla[2, 0] = "1";
            tabla[2, 1] = "d1";
            tabla[2, 2] = "d2";
            tabla[2, 3] = "d3";
            tabla[2, 4] = "error";
            tabla[2, 5] = "5";

            //Cuarto Renglón
            tabla[3, 0] = "2";
            tabla[3, 1] = "d1";
            tabla[3, 2] = "d2";
            tabla[3, 3] = "d3";
            tabla[3, 4] = "error";
            tabla[3, 5] = "6";

            //Quinto Renglón
            tabla[4, 0] = "3";
            tabla[4, 1] = "r3";
            tabla[4, 2] = "r3";
            tabla[4, 3] = "";
            tabla[4, 4] = "r3";
            tabla[4, 5] = "error";

            //Sexto Renglón
            tabla[5, 0] = "4";
            tabla[5, 1] = "error";
            tabla[5, 2] = "error";
            tabla[5, 3] = "error";
            tabla[5, 4] = "AC";
            tabla[5, 5] = "error";

            //Septimo Renglón
            tabla[6, 0] = "5";
            tabla[6, 1] = "d7";
            tabla[6, 2] = "error";
            tabla[6, 3] = "error";
            tabla[6, 4] = "error";
            tabla[6, 5] = "error";

            //Octavo Renglón
            tabla[7, 0] = "6";
            tabla[7, 1] = "error";
            tabla[7, 2] = "d8";
            tabla[7, 3] = "error";
            tabla[7, 4] = "error";
            tabla[7, 5] = "error";

            //Noveno Renglón
            tabla[8, 0] = "7";
            tabla[8, 1] = "r1";
            tabla[8, 2] = "r1";
            tabla[8, 3] = "error";
            tabla[8, 4] = "r1";
            tabla[8, 5] = "error";

            //Decimo Renglón
            tabla[9, 0] = "8";
            tabla[9, 1] = "r2";
            tabla[9, 2] = "r2";
            tabla[9, 3] = "error";
            tabla[9, 4] = "r2";
            tabla[9, 5] = "error";

            return tabla;
        }

        private string Accion(int s, string a, string[,] tabla)
        {
            int columna = 0;
            int renglon = 0;
            string resultado = "";

            for (int i = 0; i < 10; i++)
                if (tabla[i, 0].ToString().Equals(s.ToString()))
                {
                    renglon = i;
                    i = 10;
                }

            for (int i = 0; i < 6; i++)
                if (tabla[0, i].ToString().Equals(a.ToString()))
                {
                    columna = i;
                    i = 6;
                }

            resultado = tabla[renglon, columna];
            return resultado;
        }

        private List<int> pop(int length, List<int> p)
        {
            for (int i = 0; i < length; i++)
                p.RemoveAt(p.Count - 1);
            return p;
        }

        [HttpPost]
        public ActionResult AnalizadorSintacticoLR1(string gram)
        {

            List<Dictionary<int, List<string>>> todo = new List<Dictionary<int, List<string>>>();
            Dictionary<string, List<string>> gramatica = CreaDiccionario(gram);
            Dictionary<int, List<string>> estados = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> tablaDeTransicion = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> DiccionarioSignos = new Dictionary<int, List<string>>();
            List<string> signosGramaticales = new List<string>();            
            List<string> produccionAumentada = GeneraAumentada(gramatica);
            List<string> I = new List<string>();
            //string[,] tabla_analisis_lr1;
            I = Cerradura(gramatica, produccionAumentada);
            estados.Add(estados.Count, I);
            signosGramaticales = GetSignosGramaticales(gramatica);
            GetEstados(gramatica, estados, tablaDeTransicion, signosGramaticales);
            
            DiccionarioSignos.Add(0, signosGramaticales);
            todo.Add(estados);
            todo.Add(tablaDeTransicion);
            todo.Add(DiccionarioSignos);
            /*
            string[,] tabla_tans = Tabla_De_Transicion(estados,tablaDeTransicion,signosGramaticales);
            tabla_analisis_lr1 = Tabla_De_Analsis_LR1(estados, signosGramaticales,tabla_tans,gramatica);
            Analizador_De_Cadena_LR1("i", gramatica,tabla_analisis_lr1,estados.Count+1,signosGramaticales.Count+2);*/
            return View("AnalizadorSintacticoLR1", todo);
        }

        private void checaLista()
        {

            List<int> a = new List<int>();
            List<int> b = new List<int>();
            bool c = false;
            for (int i = 0; i < 5; i++)
                a.Add(i);

            for (int i = 4; i > -1 ; i--)
                b.Add(i);
            if(a.SequenceEqual(b))
            {
                c = true;
            }
        }
        private Dictionary<string, List<string>> CreaDiccionario(string gram)
        {
            Dictionary<string, List<string>> producciones = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            List<string> value = new List<string>();
            string[] partes;
            string key = "";
            partes = gram.Split('\n');
            for (int i = 0; i < partes.Length; i++)
            {
                string temp = partes[i];
                string temp2 = "";
                string[] key2temp = temp.Split('-');
                key = temp[0].ToString();
                if (key2temp[0].Length == 1 && IsAllUpper(key))
                {
                    if (temp.Contains("&"))
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {
                            if (temp[j].Equals('&'))
                                temp2 += "Ɛ";
                            else
                                temp2 += temp[j];
                        }
                        temp = "";
                        temp = temp2;
                    }
                    string[] valores = temp.Split('>');
                    string[] valorIndividual = valores[1].Split('|');
                    if (producciones.ContainsKey(key))
                    {
                        for (int k = 0; k < producciones[key].Count; k++)
                            value.Add(producciones[key][k]);

                        for (int k = 0; k < valorIndividual.Length; k++)
                            if (!value.Contains(valorIndividual[k]))
                                value.Add(valorIndividual[k]);

                        producciones[key] = value;

                        value = new List<string>();
                    }
                    else
                    {
                        for (int j = 0; j < valorIndividual.Length; j++)
                            value.Add(valorIndividual[j]);
                        producciones.Add(key, value);
                        value = new List<string>();
                    }

                }
                else
                {
                    producciones = new Dictionary<string, List<string>>();
                    producciones.Add("X", new List<string> { "ERROR" });
                }
            }
            return producciones;
        }
        private List<string> GeneraAumentada(Dictionary<string, List<string>> prod)
        {
            List<string> llaves = new List<string>(prod.Keys);
            List<string> aumentadaLista = new List<string>();
            string aumentada = llaves[0] + "'->." + llaves[0] + ",$";
            aumentadaLista.Add(aumentada);
            return aumentadaLista;
        }
        private List<string> Cerradura(Dictionary<string, List<string>> gram, List<string> elementos)
        {
            List<string> I = new List<string>();
            List<string> producciones = new List<string>();
            string nuevaProduccion = "";
            foreach (string s in elementos)
                I.Add(s);

            for (int i = 0; i < I.Count; i++)
                if (NoTerminalDespuesDelPunto(I[i]) == true)
                {
                    producciones = ObtenerProduccionesDelNoTerminal(gram, ObtenerNoTerminal(I[i]));
                    List<string> b = ObtenPrimeroLR1(gram, I[i]);

                    for (int j = 0; j < producciones.Count; j++) // ITera las producciones del no terminal
                        for (int k = 0; k < b.Count; k++)
                        {
                            if (producciones[j].ToString().Equals("Ɛ".ToString()))
                                producciones[j] = "";
                            nuevaProduccion = ObtenerNoTerminal(I[i]) + "->." + producciones[j] + "," + b[k];
                            if (!I.Contains(nuevaProduccion))
                                I.Add(nuevaProduccion);
                            nuevaProduccion = "";
                        }
                }
            return I;
        }
        private bool NoTerminalDespuesDelPunto(string produccion)
        {
            int indiceDelPunto = produccion.IndexOf(".");
            if (char.IsUpper(produccion[indiceDelPunto + 1])) // Si hay un no terminal después del punto
                return true;
            else
                return false;
        }
        private string ObtenerNoTerminal(string produccion)
        {
            int indiceDelPunto = produccion.IndexOf(".");
            return produccion[indiceDelPunto + 1].ToString();
        }
        private List<string> ObtenerProduccionesDelNoTerminal(Dictionary<string, List<string>> gram, string encabezado)
        {
            List<string> produccionesDelEncabezado = new List<string>();
            for (int i = 0; i < gram[encabezado].Count; i++)
                produccionesDelEncabezado.Add(gram[encabezado][i]);
            return produccionesDelEncabezado;
        }

        private List<string> ObtenPrimeroLR1(Dictionary<string, List<string>> grama, string produccion)
        {
            List<string> primeros = new List<string>();
            string beta = "";
            int indicePunto = produccion.IndexOf(".");
            int indiceComa = produccion.IndexOf(",");

            for (int i = indicePunto + 2; i < indiceComa; i++)
                beta += produccion[i].ToString();
            beta += produccion[indiceComa + 1].ToString();

            if (beta.Length == 0)
                primeros.Add(produccion[indiceComa + 1].ToString());
            else
            {
                if (!char.IsUpper(beta[0]))
                    primeros.Add(beta[0].ToString());
                else
                {
                    if (beta[0].ToString().Equals("Ɛ".ToString()))
                        primeros.Add(produccion[indiceComa + 1].ToString());
                    else
                    {
                        Dictionary<string, List<string>> nuevaGram = new Dictionary<string, List<string>>();
                        List<string> betaLista  = new List<string>();
                        betaLista.Add(beta);
                        foreach (KeyValuePair<string, List<string>> entry in grama)
                            nuevaGram.Add(entry.Key, entry.Value);

                        nuevaGram.Add("key", betaLista);
                        primeros = plr1(nuevaGram);//ObtenPrimero(nuevaGram, beta[0].ToString());
                    }
                    
                }
            }
            return primeros;
        }

        private List<string> GetSignosGramaticales(Dictionary<string, List<string>> grama)
        {
            List<string> signosG = new List<string>();

            foreach (KeyValuePair<string, List<string>> d in grama)
                foreach (string s in d.Value)
                    foreach (char c in s)
                        if (!c.ToString().Equals("Ɛ".ToString()))
                            if (!signosG.Contains(c.ToString()))
                                signosG.Add(c.ToString());
            List<string> llaves = new List<string>(grama.Keys);

            foreach (string s in llaves)
                if (!signosG.Contains(s))
                    signosG.Add(s);

            return signosG;
         }

        private void GetEstados(Dictionary<string, List<string>> gram, Dictionary<int, List<string>> edos, Dictionary<int, List<string>> tablaD, List<string> signos)
        {
            List<int> llaves = new List<int>(edos.Keys);
            for (int i = 0; i < llaves.Count; i++)//each (KeyValuePair<int, List<string>> d in edos)//ITera las llaves 
            {
                foreach (string signo in signos)// por cada signo gramatical
                {
                    List<string> J = Ir_ALR1(gram, edos[llaves[i]], signo,llaves[i],tablaD,edos);
                    if (J.Count != 0 && NoEstaEnC(J, edos) == true)
                    {
                        edos.Add(edos.Count, J);
                        AgregarDatosATablaDeTransicion(tablaD, llaves[i], signo, edos.Count-1);
                    }
                    else
                    {
                        if(J.Count != 0 && NoEstaEnC(J,edos) == false) // si el elementos mas mayor de cero y se repite 
                            AgregarDatosATablaDeTransicion(tablaD, llaves[i], signo, GetDestino(edos, J));                        
                    }
                }
                llaves = new List<int>(edos.Keys);
            }
        }

        private List<string> Ir_ALR1(Dictionary<string, List<string>> gramatica, List<string> i, string signoG,int numEstado, Dictionary<int, List<string>> trans, Dictionary<int, List<string>> estados)
        {
            List<string> cerradura = new List<string>();
            List<string> j = new List<string>();
            foreach (string s in i)
                if (SignoGRamaticalDespuesDelPunto(s, signoG))
                    j.Add(AvanzaPunto(s));// la produccion con el punt ovanzado                                        
            return Cerradura(gramatica, j);
        }
        private bool SignoGRamaticalDespuesDelPunto(string elemento, string signo)
        {
            int indiceSignoGramatical = elemento.IndexOf(".") + 1;
            if (elemento[indiceSignoGramatical].ToString().Equals(signo.ToString()))
                return true;
            else
                return false;
        }

        private string AvanzaPunto(string elemento)
        {
            string nuevoElemento = "";
            string signoSiguiente = "";

            int indicePunto = elemento.IndexOf(".");
            if (!elemento[indicePunto + 1].ToString().Equals(",".ToString()))
            {
                signoSiguiente = elemento[indicePunto + 1].ToString();
                for (int i = 0; i < elemento.Length; i++)
                {
                    if (indicePunto == i)
                        nuevoElemento += signoSiguiente;
                    else if (indicePunto + 1 == i)
                        nuevoElemento += ".";
                    else
                        nuevoElemento += elemento[i];
                }
            }
            return nuevoElemento;
        }

        private bool NoEstaEnC(List<string> nuevoElementos, Dictionary<int, List<string>> estados)
        {
            bool noEsta = true;
            List<int> llaves = new List<int>(estados.Keys);
            List< List<string>> listaDeEstados = new List<List<string>>();

            foreach (int i  in llaves)
                listaDeEstados.Add(estados[llaves[i]]);

            for (int i = 0; i < listaDeEstados.Count; i++)
            {
                nuevoElementos.Sort();
                listaDeEstados[i].Sort();
                if (nuevoElementos.SequenceEqual(listaDeEstados[i]))
                {
                    noEsta = false;
                }
            }

            return noEsta;
        }        

        private void AgregarDatosATablaDeTransicion(Dictionary<int, List<string>> transicion, int numeroDeEstado, string signoG, int estadoDestino)
        {
            List<string> conexiones = new List<string>();

            if(!transicion.ContainsKey(numeroDeEstado))//si no contiene la llave
            {
                conexiones.Add(signoG + "-" + estadoDestino);
                transicion.Add(numeroDeEstado, conexiones);
            }
            else
            {
                string nuevaConexion = signoG + "-" + estadoDestino;
                foreach (string s in transicion[numeroDeEstado])
                    conexiones.Add(s);
                if (!conexiones.Contains(nuevaConexion))
                    conexiones.Add(nuevaConexion);
                transicion[numeroDeEstado] = conexiones;
            }
        }

        private int GetDestino(Dictionary<int,List<string>> estados,List<string> nuevoElementos)
        {
            int destino=0;
            List<int> llaves = new List<int>(estados.Keys);
            List<List<string>> listaDeEstados = new List<List<string>>();

            foreach (int i in llaves)
                listaDeEstados.Add(estados[llaves[i]]);

            for (int i = 0; i < listaDeEstados.Count; i++)
            {
                if (nuevoElementos.SequenceEqual(listaDeEstados[i]))
                {
                    destino = i;
                    i = listaDeEstados.Count;
                }
            }

            return destino;
        }


        private List<string> plr1(Dictionary<string, List<string>> produc)
        {
            List<string> llaves = new List<string>(produc.Keys);
            List<string> primerosLista = new List<string>();
            List<string> obtenidos = new List<string>();
            List<string> primerosRec = new List<string>();
            Dictionary<string, List<string>> primeros = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> prod = new Dictionary<string, List<string>>();
            List<string> produccionRecLista = new List<string>();
            //Dictionary<string, List<string>> recursivo = new Dictionary<string, List<string>>();
            List<string> Recursivas = new List<string>();
            prod = ordenaDiccionario(produc);

            int k;
            bool continuar;
            for (int i = 0; i < llaves.Count; i++)//recorre cade llave
            {
                for (int j = 0; j < prod[llaves[i]].Count; j++)//Recorre cada produccion de la llave
                {
                    k = 0; continuar = true;
                    while (continuar == true && k < prod[llaves[i]][j].Length /*int k = 0; k < prod[llaves[i]][j].Length; k++*/)//Recorre cada caracter de la produccion
                    {
                        if (IsAllUpper(prod[llaves[i]][j][k].ToString()))//SI EL caracter es un no terminal
                        {
                            if (llaves[i].Equals(prod[llaves[i]][j][k].ToString()))
                            {
                                //llaveRecursivas.Ad(llaves[i]);       //SI SE HACE AQUI                         
                                primerosRec = Recursivo(prod, llaves[i]);
                                //
                                for (int l = 0; l < primerosRec.Count; l++)
                                {
                                    if (!primerosRec[l].Equals("Ɛ"))
                                        if (!primerosLista.Contains(primerosRec[l]))
                                            primerosLista.Add(primerosRec[l]);
                                }
                                continuar = false;
                            }
                            else
                            {
                                obtenidos = ObtenPrimero(prod, prod[llaves[i]][j][k].ToString());
                                for (int l = 0; l < obtenidos.Count; l++)
                                {
                                    if (!obtenidos[l].Equals("Ɛ"))
                                        if (!primerosLista.Contains(obtenidos[l]))
                                            primerosLista.Add(obtenidos[l]);
                                }
                                if (!obtenidos.Contains("Ɛ"))
                                    continuar = false;
                            }

                        }
                        else///SI EL CARACTER ES UN TERMINAL 
                        {
                            primerosLista.Add(prod[llaves[i]][j][k].ToString());
                            continuar = false;
                        }
                        k++;
                    }
                    if (continuar)
                        primerosLista.Add("Ɛ");
                }
                primeros.Add(llaves[i], primerosLista);
                primerosLista = new List<string>();

            }
            //Recursivo(prod,);//mandar el diccionario completo y la llave 

            //agregar los no terminales al diccionario de primeros
            //primeros = AgregaTerminalesAPrimeros(primeros, produc);
        
            List<string> primlr1 = new List<string>();        
            foreach (KeyValuePair<string, List<string>> entry in primeros)
                foreach (string s in entry.Value)
                    primlr1.Add(s);
            
            return primlr1;
        }

        private string[,] Tabla_De_Transicion(Dictionary<int,List<string>> edos, Dictionary<int, List<string>> destino, List<string> sg)
        {
            Dictionary<int,List<string>> transiciones = destino;
            string[,] tabla_transicion = new string[edos.Count+1, sg.Count+1];

            for(int i = 1; i < edos.Count+1;i++)
                for (int j = 1; j < sg.Count+1; j++)
                    tabla_transicion[i, j] = "φ";

            for(int i = 0;  i< sg.Count; i++)
                tabla_transicion[0, i+1] = sg[i];

            int k = 1;
            foreach(KeyValuePair<int, List<string>> entry in edos)            
            {
                tabla_transicion[k, 0] = "I"+entry.Key.ToString();
                k++;
            }


            for (int i = 0; i < edos.Count; i++)
                for (int j = 0; j < sg.Count; j++)
                    foreach (KeyValuePair<int, List<string>> entry in destino)//itera las llaves del diccionario
                        if (tabla_transicion[i+1, 0].ToString().Equals("I"+entry.Key.ToString()))
                            for (int l = 0; l < entry.Value.Count; l++)
                            {
                                string[] des = entry.Value[l].Split('-');
                                if (des[0].ToString().Equals(tabla_transicion[0, j+1].ToString()))
                                    tabla_transicion[i+1, j+1] = des[1].ToString();
                            }

            tabla_transicion[0, 0] = "--";
            return tabla_transicion;
        }

        private string[,] Tabla_De_Analsis_LR1(Dictionary<int,List<string>> edos,List<string> sg, string[,] tabla_tr, Dictionary<string, List<string>> gram)
        {
            int k = 1;
            string[,] tabla_analisis = new string[edos.Count + 1, sg.Count + 2];
            List<string> signos = new List<string>();
            List<string> no_terminales = new List<string>();

            for (int i = 0; i < edos.Count + 1; i++)
                for (int j = 0; j < sg.Count + 2; j++)
                    tabla_analisis[i, j] = "error";

            tabla_analisis[0,0] = "Estados";


            foreach (KeyValuePair<int, List<string>> entry in edos)
            {
                tabla_analisis[k, 0] = entry.Key.ToString();
                k++;
            }            

            foreach (string s in sg)
            {
                if (char.IsUpper(s[0]))
                    no_terminales.Add(s);
                else
                    signos.Add(s);
            }

            signos.Add("$");
            foreach (string s in no_terminales)
                signos.Add(s);
            
            
            for (int i = 0; i < signos.Count; i++)
                tabla_analisis[0, i + 1] = signos[i];
            
            foreach(KeyValuePair<int, List<string>> entry in edos)
            {
                foreach (string s in entry.Value)
                {
                    if(Terminal_Despues_Del_Punto(s))
                    {
                        int indice_punto = s.LastIndexOf(".");
                        string trans = s[indice_punto + 1].ToString();

                        for(int i = 0; i < edos.Count;i++)
                            for (int j = 0; j < sg.Count + 2; j++)
                                if (entry.Key.ToString().Equals(tabla_analisis[i, 0].ToString()) && trans.ToString().Equals(tabla_analisis[0, j].ToString()))
                                    tabla_analisis[i, j] = "d"+IR_A_LR1(tabla_tr, edos.Count + 1, sg.Count + 2, trans, entry.Key.ToString());
                        
                    }
                    else if(Punto_Al_Final_Con_Excepcion_Aumentada(s))
                    {
                        int indice_coma = s.LastIndexOf(",");
                        string trans = s[indice_coma + 1].ToString();

                        for (int i = 0; i < edos.Count+1; i++)
                        {
                            for (int j = 0; j < sg.Count + 2; j++)
                            {
                                if (entry.Key.ToString().Equals(tabla_analisis[i, 0].ToString()) && trans.ToString().Equals(tabla_analisis[0, j].ToString()))
                                    tabla_analisis[i, j] = "r" + Numero_De_Produccion_LR1(s, gram);
                            }
                        }
                    }
                    else if(Punto_Al_Final_Aumentada(s))
                    {                        
                        string trans = "$";

                        for (int i = 0; i < edos.Count; i++)
                            for (int j = 0; j < sg.Count + 2; j++)
                                if (entry.Key.ToString().Equals(tabla_analisis[i, 0].ToString()) && trans.ToString().Equals(tabla_analisis[0, j].ToString()))
                                    tabla_analisis[i, j] = "AC";
                    }
                }
            }

            for(int i = 1; i < edos.Count+1;i++)//itera los renglones de la tabla de transicions
                for (int j = 0; j < signos.Count; j++)//itera los renglones de la tabla de transiciones
                    for (int s = 0; s < no_terminales.Count; s++)//itera los no terminales
                        if (tabla_tr[0, j].ToString().Equals(no_terminales[s].ToString()) && !tabla_tr[i, j].ToString().Equals("φ".ToString()))
                            for (int m = 0; m < signos.Count + 1; m++)//itera los renglones de la tabla de analisis
                                if (tabla_analisis[0, m].ToString().Equals(no_terminales[s].ToString()) /*&& !tabla_tr[i, j].ToString().Equals("φ".ToString())*/)
                                    tabla_analisis[i, m] = tabla_tr[i, j].ToString();            

            return tabla_analisis;
        }

        private bool Terminal_Despues_Del_Punto(string prod)
        {
            int indice_punto = prod.LastIndexOf(".");
            if (char.IsUpper(prod[indice_punto + 1]) || prod[indice_punto + 1].ToString().Equals(",".ToString()))
                return false;
            else
                return true;
        }

        private string IR_A_LR1(string[,] tabla_t,int renglones,int columnas,string transicion,string inicio)
        {
            string destino="";
            int i = 0;
            int j = 0;
            for(int k  = 0;  k < renglones;k++)
                if (tabla_t[k, 0].ToString().Equals("I"+inicio.ToString()))
                {
                    i = k;
                    k = renglones;
                }

            for (int k = 0; k < columnas; k++)
            {
                string transi = tabla_t[0, k].ToString();
                if (transi.ToString().Equals(transicion.ToString()))
                {
                    j = k;
                    k = columnas;
                }
            }
            destino = tabla_t[i, j];
            return destino;
        }

        private bool Punto_Al_Final_Con_Excepcion_Aumentada(string prod)
        {
            int indice_punto = prod.IndexOf(".");

            if (prod[indice_punto + 1].ToString().Equals(",".ToString()) && !prod[1].ToString().Equals("'".ToString()))
                return true;
            else
                return false;
        }

        private string Numero_De_Produccion_LR1(string prod,Dictionary<string,List<string>> grama)
        {
            string nueva_produccion = "";
            string numero_prod = "";
            List<string> lista_producciones = new List<string>();
            for(int i = 0; i < prod.Length;i++)
            {
                if (!prod[i].ToString().Equals(".".ToString()))
                    nueva_produccion += prod[i];
                else
                    i = prod.Length;
            }
            if (nueva_produccion[nueva_produccion.Length - 1].ToString().Equals(">".ToString()))
                nueva_produccion += "Ɛ";

            foreach(KeyValuePair<string,List<string>> entry in grama)
                foreach (string s in entry.Value)
                    lista_producciones.Add(entry.Key+"->"+s);

            for (int i = 0; i < lista_producciones.Count; i++)
                if (lista_producciones[i].ToString().Equals(nueva_produccion.ToString()))
                    numero_prod = (i+1).ToString();
            return numero_prod;
        }

        private bool Punto_Al_Final_Aumentada(string prod)
        {
            int indice_punto = prod.IndexOf(".");
            if (prod[indice_punto + 1].ToString().Equals(",".ToString()) && prod[1].ToString().Equals("'".ToString()))
                return true;
            else
                return false;
        }


        private List<string> Analizador_De_Cadena_LR1(string cadena,Dictionary<string,List<string>> grama,string[,] tabla_analisis,int num_renglones_tab_analsis,int num_columnas_tab_analalisis)
        {
            //Dictionary<string, List<string>> gramatica = new Dictionary<string, List<string>>();
            List<int> pila = new List<int>();
            string[,] tablaLR = new string[10, 6];
            string cadenaOriginal = cadena;
            string cadenaAccion = "";
            int s = 0;
            int t = 0;
            int indiceA = 0;
            string a = "";
            string resultado = "";
            bool salida = false;
            cadena += "$";
            pila.Add(0);
            a = cadena[indiceA].ToString();
            tablaLR = tabla_analisis;//LlenaTablaLR();

            //gramatica.Add("P", new List<string> { "aPa", "bPb", "c" });
            while (salida != true)
            {
                s = pila[pila.Count - 1]; // s apunta al tope de la pila
                cadenaAccion = Accion_LR1(s, a, tablaLR,num_renglones_tab_analsis,num_columnas_tab_analalisis);
                if (cadenaAccion[0].ToString().Equals("d".ToString()))
                {
                    int indice_d = cadenaAccion.IndexOf("d");
                    string t_tmp = "";
                    for (int i = (indice_d + 1); i < cadenaAccion.Length; i++)
                        t_tmp += cadenaAccion[i];

                    int.TryParse(t_tmp, out t);
                    //int.TryParse(cadenaAccion[1].ToString(), out t);
                    pila.Add(t);
                    indiceA++;
                    a = cadena[indiceA].ToString();
                }
                else if (cadenaAccion[0].ToString().Equals("r".ToString()))
                {
                    int indice_r = cadenaAccion.IndexOf("r");
                    string r_tmp = "";
                    for (int i = (indice_r + 1); i < cadenaAccion.Length; i++)
                        r_tmp += cadenaAccion[i];

                    int numProd, prodLength;
                    int.TryParse(r_tmp, out numProd);
                    //int.TryParse(cadenaAccion[1].ToString(), out numProd);
                    int irA;
                    List<string> lista_llaves = new List<string>();
                    List<string> lista_producciones = new List<string>();
                    //buscar la produccion para sacar su longuitud
                    foreach (KeyValuePair<string, List<string>> entry in grama)
                        foreach (string l in entry.Value)
                        {
                            //llaveP = entry.Key.ToString();
                            lista_llaves.Add(entry.Key.ToString());
                            lista_producciones.Add(l);
                        }
                    if (lista_producciones[numProd - 1].ToString().Equals("Ɛ".ToString()))
                    {
                        prodLength = 0;
                    }
                    else
                    {
                        prodLength = lista_producciones[numProd - 1].Length;//gramatica["P"][numProd - 1].Length;
                    }
                    pila = pop(prodLength, pila);
                    t = pila[pila.Count - 1];

                    int.TryParse(Accion_LR1(t, lista_llaves[numProd-1], tablaLR, num_renglones_tab_analsis, num_columnas_tab_analalisis), out irA);
                    pila.Add(irA);
                }
                else if (cadenaAccion.ToString().Equals("AC".ToString()))
                {
                    resultado = "La cadena '" + cadenaOriginal + "' si pertenece al lenguaje de la gramática";
                    salida = true;
                }
                else
                {
                    resultado = "La cadena '" + cadenaOriginal + "' no pertenece al lenguaje de la gramática";
                    salida = true;
                }
            }
            List<string> res = new List<string>();
            res.Add(resultado);
            return res;
        }


        private string Accion_LR1(int s, string a, string[,] tabla,int num_renglones_tabla_analisis, int num_columnas_tabla_analisis)
        {
            int columna = 0;
            int renglon = 0;
            string resultado = "";

            for (int i = 0; i < num_renglones_tabla_analisis; i++)
                if (tabla[i, 0].ToString().Equals(s.ToString()))
                {
                    renglon = i;
                    i = num_renglones_tabla_analisis;
                }

            for (int i = 0; i < num_columnas_tabla_analisis; i++)
                if (tabla[0, i].ToString().Equals(a.ToString()))
                {
                    columna = i;
                    i = num_columnas_tabla_analisis;
                }

            resultado = tabla[renglon, columna];
            return resultado;
        }

        [HttpPost]
        public ActionResult AnalizadorDeCadenaLR1(string gram)
        {
            string[] conjunto = gram.Split('°');

            List<Dictionary<int, List<string>>> todo = new List<Dictionary<int, List<string>>>();
            Dictionary<string, List<string>> gramatica = CreaDiccionario(conjunto[0]);
            Dictionary<int, List<string>> estados = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> tablaDeTransicion = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> DiccionarioSignos = new Dictionary<int, List<string>>();
            List<string> signosGramaticales = new List<string>();
            List<string> produccionAumentada = GeneraAumentada(gramatica);
            List<string> I = new List<string>();
            List<string> resultado = new List<string>();
            string[,] tabla_analisis_lr1;
            I = Cerradura(gramatica, produccionAumentada);
            estados.Add(estados.Count, I);
            signosGramaticales = GetSignosGramaticales(gramatica);
            GetEstados(gramatica, estados, tablaDeTransicion, signosGramaticales);

            DiccionarioSignos.Add(0, signosGramaticales);
            todo.Add(estados);
            todo.Add(tablaDeTransicion);
            todo.Add(DiccionarioSignos);

            string[,] tabla_tans = Tabla_De_Transicion(estados, tablaDeTransicion, signosGramaticales);
            tabla_analisis_lr1 = Tabla_De_Analsis_LR1(estados, signosGramaticales, tabla_tans, gramatica);
            resultado = Analizador_De_Cadena_LR1(conjunto[1], gramatica, tabla_analisis_lr1, estados.Count + 1, signosGramaticales.Count + 2);
            List<object> sender = new List<object>() { tabla_analisis_lr1, estados.Count + 1, signosGramaticales.Count + 2 , resultado[0]};
            //tabla_analisis_lr1[0, 0] = resultado[0];
            //object tabla = tabla_analisis
            return View("analizador", sender);
        }
    }

}