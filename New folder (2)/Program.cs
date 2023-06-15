using System.IO;
using System.Text.Json;

Phone myPhone = new Phone("1234");
myPhone.LoadContactsFromJson();
myPhone.LoadMessagesFromJson();
Console.WriteLine("The password of the phone is: " + myPhone.GetPassword() + " so that Mr Colin Veldkamp knows the password");



while (true)
{
    Console.WriteLine("Phone menu");
    Console.WriteLine("1 unlock phone");
    Console.WriteLine("2 exit");
    Console.Write("Enter your choice: ");
    int choice;
    if (!int.TryParse(Console.ReadLine(), out choice))
    {
        Console.WriteLine("Invalid choice please try again");
        continue;
    }            
    

    switch (choice)
    {
        case 1:
            UnlockPhone(myPhone);
            break;
        case 2:
            Console.WriteLine("Exiting");
            return;
        default:
            Console.WriteLine("Invalid choice please try again");
            break;
    }
}

static void UnlockPhone(Phone phone)
{
    if (phone.IsLocked())
    {
        Console.Write("Enter the password: ");
        string enteredPassword = Console.ReadLine();
        phone.Unlock(enteredPassword);

        if (!phone.IsLocked())
        {
            while (true)
            {
                Console.WriteLine("\nPhone Menu");
                Console.WriteLine("1 Change Password");
                Console.WriteLine("2 Photo App");
                Console.WriteLine("3 Contact App");
                Console.WriteLine("4 Messaging App");
                Console.WriteLine("5 Exit");
                Console.Write("Enter your choice: ");
                int submenuChoice;
                if (!int.TryParse(Console.ReadLine(), out submenuChoice))
                {
                    Console.WriteLine("Invalid choice please try again.");
                    continue;
                }


                switch (submenuChoice)
                {
                    case 1:
                        ChangePassword(phone);
                        break;
                    case 2:
                        RunPhotoApp(phone);
                        break;
                    case 3:
                        RunContactApp(phone);
                        break;
                    case 4:
                        RunMessagingApp(phone);
                        break;
                    case 5:
                        Console.WriteLine("exiting");
                        phone.Lock();
                        return;
                    default:
                        Console.WriteLine("Invalid choice lease try again.");
                        break;
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Phone is already unlocked");
    }
}

static void ChangePassword(Phone phone)
{
    Console.Write("Enter a new password: ");
    string newPassword = Console.ReadLine();
    phone.SetPassword(newPassword);
}

static void RunPhotoApp(Phone phone)
{
    while (true)
    {
        Console.WriteLine("\nPhoto App");
        Console.WriteLine("1. Click Photo");
        Console.WriteLine("2. View Photos");
        Console.WriteLine("3. Go Back");
        Console.Write("Enter your choice: ");
        int photoChoice;
        if (!int.TryParse(Console.ReadLine(), out photoChoice))
        {
            Console.WriteLine("Invalid choice please try again");
            continue;
        }

        switch (photoChoice)
        {
            case 1:
                phone.ClickPhoto();
                break;
            case 2:
                phone.ViewPhotos();
                break;
            case 3:
                return;
            default:
                Console.WriteLine("Invalid choice please try again.");
                break;
        }
    }
}

static void RunContactApp(Phone phone)
{
    while (true)
    {
        Console.WriteLine("\nContact App");
        Console.WriteLine("1 Add Contact");
        Console.WriteLine("2 Remove Contact");
        Console.WriteLine("3 Edit Contact");
        Console.WriteLine("4 View Contacts");
        Console.WriteLine("5 Go Back");
        Console.Write("Enter your choice: ");
        int contactChoice;
        if (!int.TryParse(Console.ReadLine(), out contactChoice))
        {
            Console.WriteLine("Invalid choice please try again.");
            continue;
        }

        switch (contactChoice)
        {
            case 1:
                AddContact(phone);
                break;
            case 2:
                RemoveContact(phone);
                break;
            case 3:
                EditContact(phone);
                break;
            case 4:
                phone.ViewContacts();
                break;
            case 5:
                return;
            default:
                Console.WriteLine("Invalid choice please try again.");
                break;
        }
    }
}

static void AddContact(Phone phone)
{
    Console.Write("Enter the contact name: ");
    string name = Console.ReadLine();
    Console.Write("Enter the contact number: ");
    string number = Console.ReadLine();
    phone.AddContact(name, number);
}

static void RemoveContact(Phone phone)
{
    Console.Write("Enter the contact name to remove: ");
    string name = Console.ReadLine();
    phone.RemoveContact(name);
}

static void EditContact(Phone phone)
{
    Console.Write("Enter the contact name to edit: ");
    string name = Console.ReadLine();
    Console.Write("Enter the new contact number: ");
    string newNumber = Console.ReadLine();
    phone.EditContact(name, newNumber);
}

static void RunMessagingApp(Phone phone)
{
    while (true)
    {
        Console.WriteLine("\nMessaging App");
        Console.WriteLine("1 Send Message");
        Console.WriteLine("2 View Messages");
        Console.WriteLine("3 Go Back");
        Console.Write("Enter your choice: ");
        int messagingChoice;
        if (!int.TryParse(Console.ReadLine(), out messagingChoice))
        {
            Console.WriteLine("Invalid choice please try again.");
            continue;
        }

        switch (messagingChoice)
        {
            case 1:
                SendMessage(phone);
                break;
            case 2:
                phone.ViewMessages();
                break;
            case 3:
                return;
            default:
                Console.WriteLine("Invalid choice please try again.");
                break;
        }
    }
}

static void SendMessage(Phone phone)
{
    Console.Write("Enter the contact name: ");
    string contactName = Console.ReadLine();
    Console.Write("Enter the message content: ");
    string content = Console.ReadLine();
    phone.SendMessage(contactName, content);
}

class Phone
{
    private string password;
    private bool isLocked;
    private List<string> photos;
    private List<Contact> contacts;
    private List<Message> messages;
    private const string contactsFilePath = "contacts.json";
    private const string messagesFilePath = "messages.json";
    private const string passwordFilePath = "password.json";

    public Phone(string password)
    {
        this.password = LoadPasswordFromJson() ?? password;
        isLocked = true;
        photos = new List<string>();
        contacts = new List<Contact>();
        messages = new List<Message>();
    }

    public string GetPassword()
    {
        return password;
    }

    public void SetPassword(string newPassword)
    {
        password = newPassword;
        Console.WriteLine("Password set successfully");
        SavePasswordToJson(password);
    }

    public void Unlock(string enteredPassword)
    {
        if (enteredPassword == password)
        {
            isLocked = false;
            Console.WriteLine("Phone unlocked");
        }
        else
        {
            Console.WriteLine("Invalid password try agian");
        }
    }
        
    public void Lock()
    {
        isLocked=true;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public void ClickPhoto()
    {
        Console.WriteLine("Clicking photo");
        string photo = $"Photo {photos.Count + 1}";
        photos.Add(photo);
        Console.WriteLine("Photo saved to gallery");
    }

    public void ViewPhotos()
    {
        Console.WriteLine("Here are the photos:");
        foreach (string photo in photos)
        {
            Console.WriteLine(photo);
        }
    }

    public void AddContact(string name, string number)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number))
        {
            Contact contact = new Contact(name, number);
            contacts.Add(contact);
            Console.WriteLine("Contact added successfully");
            SaveContactsToJson();
        }
        else
        {
            Console.WriteLine("Invalid name for contact");
        }
    }

    public void RemoveContact(string name)
    {
        Contact contactToRemove = contacts.Find(c => c.Name == name);
        if (contactToRemove != null)
        {
            contacts.Remove(contactToRemove);
            Console.WriteLine("Contact removed successfully");
            SaveContactsToJson();
        }
        else
        {
            Console.WriteLine("Contact not found");
        }
    }

    public void EditContact(string name, string newNumber)
    {
        Contact contactToEdit = contacts.Find(c => c.Name == name);
        if (contactToEdit != null)
        {
            contactToEdit.Number = newNumber;
            Console.WriteLine("Contact edited successfully");
            SaveContactsToJson();
        }
        else
        {
            Console.WriteLine("Contact not found");
        }
    }

    public void ViewContacts()
    {
        Console.WriteLine("Here are the contacts:");
        foreach (Contact contact in contacts)
        {
            Console.WriteLine($"Name: {contact.Name}, Number: {contact.Number}");
        }
    }

    public void SendMessage(string contactName, string content)
    {
        Contact contact = contacts.Find(c => c.Name == contactName);
        if (contact != null)
        {
            Message message = new Message(contact, content);
            messages.Add(message);
            Console.WriteLine("Message sent successfully");
            SaveMessagesToJson();
        }
        else
        {
            Console.WriteLine("Contact not found");
        }
    }

    public void ViewMessages()
    {
        if (messages.Count > 0)
        {
            Console.WriteLine("Here are the messages:");
            foreach (Message message in messages)
            {
                Console.WriteLine($"Sent TO: {message.Contact.Name}, Content: {message.Content}");
            }
        }
        else
        {
            Console.WriteLine("No messages found.");
        }
    }

    public void SaveContactsToJson()
    {
        string jsonData = JsonSerializer.Serialize(contacts);
        File.WriteAllText(contactsFilePath, jsonData);
    }

    public void LoadContactsFromJson()
    {
        if (File.Exists(contactsFilePath))
        {
            string jsonData = File.ReadAllText(contactsFilePath);
            contacts = JsonSerializer.Deserialize<List<Contact>>(jsonData) ?? new List<Contact>();
        }
    }

    public void SaveMessagesToJson()
    {
        string jsonData = JsonSerializer.Serialize(messages);
        File.WriteAllText(messagesFilePath, jsonData);
    }

    public void LoadMessagesFromJson()
    {
        if (File.Exists(messagesFilePath))
        {
            string jsonData = File.ReadAllText(messagesFilePath);
            messages = JsonSerializer.Deserialize<List<Message>>(jsonData) ?? new List<Message>();
        }
    }

    public void SavePasswordToJson(string password)
    {
        string jsonData = JsonSerializer.Serialize(password);
        File.WriteAllText(passwordFilePath, jsonData);
    }

    public string LoadPasswordFromJson()
    {
        if (File.Exists(passwordFilePath))
        {
            string jsonData = File.ReadAllText(passwordFilePath);
            return JsonSerializer.Deserialize<string>(jsonData);
        }
        return null;
    }
}

class Contact
{
    public string Name { get; set; }
    public string Number { get; set; }

    public Contact(string name, string number)
    {
        Name = name;
        Number = number;
    }
}

class Message
{
    public Contact Contact { get; set; }
    public string Content { get; set; }

    public Message(Contact contact, string content)
    {
        Contact = contact;
        Content = content;
    }
}
