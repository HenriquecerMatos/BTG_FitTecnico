namespace Applicability.Shared
{
    /// <summary>
    /// PT-BR: Classe Cliente.
    /// <para>en: Client Class.</para>
    /// </summary>

    public class ClientDTO
    {
        /// <summary>
        /// PT-BR: Nome do cliente.
        /// <para>en: Customer name.</para>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// PT-BR: Último nome do cliente.
        /// <para>en: Customer last name.</para>
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// PT-BR: Idade do cliente.
        /// <para>en: Customer age.</para>
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// PT-BR: Endereço do cliente.
        /// <para>en: Customer address.</para>
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// PT-BR: Retorna o nome completo do cliente.
        /// <para>en: Returns the customer's full name.</para>
        /// </summary>
        public string FullName { get { return Name + " " + LastName; } }
      
    }
}
