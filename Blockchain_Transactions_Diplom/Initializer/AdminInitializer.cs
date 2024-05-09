using Blockchain_Transactions_Diplom.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

public class AdminInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly RSAEncryptor _encryptor;

    public AdminInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _encryptor = new RSAEncryptor();
    }

    public async Task Initialize()
    {
        await CreateSuperAdmin();
        await CreateAdmin();
    }
    public async Task CreateAdmin()
    {
        // Проверяем наличие роли SuperAdmin
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            // Создаем роль SuperAdmin, если её нет
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        // Проверяем наличие пользователей
        var superAdmins = await _userManager.GetUsersInRoleAsync("Admin");
        if (superAdmins.Count == 0)
        {
            var keys = _encryptor.GenerateKeys();
            // Создаем пользователя с ролью SuperAdmin, если его нет
            var superAdmin = new AppUser
            {
                UserName = "dshohov@gmail.com",
                Email = "dshohov@gmail.com",
                Balance = 0,
                FirstName = "AdminCoinApp",
                LastName = "AdminCoinApp",
                Publickey = keys.Publickey,
                PrivateKey = keys.PrivateKey
            };
            var result = await _userManager.CreateAsync(superAdmin, "S1234567890Shokhov)");
            await _userManager.AddToRoleAsync(superAdmin, "Admin");
        }
    }
    public async Task CreateSuperAdmin()
    {
        // Проверяем наличие роли SuperAdmin
        if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
        {
            // Создаем роль SuperAdmin, если её нет
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        }
        // Проверяем наличие пользователей
        var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
        if (superAdmins.Count == 0)
        {
            var keys = _encryptor.GenerateKeys();
            // Создаем пользователя с ролью SuperAdmin, если его нет
            var superAdmin = new AppUser
            {
                UserName = "SuperAdminCoinApp",
                Email = GenerateRandomEmail(),
                Balance = ulong.MaxValue,
                FirstName = "SuperAdminCoinApp",
                LastName = "SuperAdminCoinApp",
                Publickey = keys.Publickey,
                PrivateKey = keys.PrivateKey
            };
            var result = await _userManager.CreateAsync(superAdmin, GetRandomPassword(256));
            await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }

    }
    public static string GenerateRandomEmail()
    {
        // Генерируем случайную строку для имени почты
        string emailName = GetRandomString(128);

        // Добавляем домен к имени почты
        string domain = "@student.csn.khai.edu";

        // Возвращаем полный случайно сгенерированный адрес почты
        return emailName + domain;
    }

    // Метод для генерации случайной строки
    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var stringBuilder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }
        return stringBuilder.ToString();
    }
    private static string GetRandomPassword(int length)
    {
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string digitChars = "0123456789";
        const string specialChars = "!@#$%^&*";

        StringBuilder passwordBuilder = new StringBuilder();

        Random random = new Random();
        // Генерируем хотя бы один символ из каждой категории
        passwordBuilder.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);
        passwordBuilder.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);
        passwordBuilder.Append(digitChars[random.Next(digitChars.Length)]);
        passwordBuilder.Append(specialChars[random.Next(specialChars.Length)]);

        // Добавляем оставшиеся символы
        for (int i = 4; i < length; i++)
        {
            string chars = uppercaseChars + lowercaseChars + digitChars + specialChars;
            passwordBuilder.Append(chars[random.Next(chars.Length)]);
        }

        // Перемешиваем символы пароля
        for (int i = 0; i < length; i++)
        {
            int swapIndex = random.Next(length);
            char temp = passwordBuilder[i];
            passwordBuilder[i] = passwordBuilder[swapIndex];
            passwordBuilder[swapIndex] = temp;
        }

        return passwordBuilder.ToString();
    }
}
