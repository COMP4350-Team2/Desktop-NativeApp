using Desktop_Frontend.DSOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;

namespace Desktop_Frontend.Backend
{
    /// <summary>
    /// Concrete implenentation of the Backend
    /// </summary>  
    public class Backend : IBackend
    {
        private BackendConfig config;

        private HttpClient HttpClient;

        private List<Ingredient> allIngredients;

        private DateTime lastAllIngCall;

        private List<string> allUnits;

        private DateTime lastAllUnitsCall;

        private List<UserList> myLists;

        private DateTime lastMyListsCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="Backend"/> class with default configuration.
        /// </summary>
        public Backend() : this(new BackendConfig()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Backend"/> class with a specified configuration.
        /// </summary>
        /// <param name="config"><see cref="BackendConfig"/> for the concrete implementation.</param>
        public Backend(BackendConfig config)
        {
            this.config = config;

            HttpClient = new HttpClient();

            allIngredients = null;

            allUnits = null;

            myLists = null;
        }

        /// <summary>
        /// Retrieves all available ingredients from the backend API.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is making requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects based on results obtained from the backend API.
        /// Shows a message box indicating failure. Empty list returned on failure.
        /// </returns>
        public async Task<List<Ingredient>> GetAllIngredients(IUser user)
        {
            // Only make the call if needed (otherwise use cached local variable)
            if (AllIngNewCall())
            {
                lastAllIngCall = DateTime.Now;
                allIngredients?.Clear();
                allIngredients = new List<Ingredient>();

                //Create request
                string url = config.BackendUrl + config.All_Ing_Endpoint;
                string accessToken = user.GetAccessToken();
                var request = new HttpRequestMessage(HttpMethod.Get, url); ;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    HttpResponseMessage response = await HttpClient.SendAsync(request);
                    ValidateAllIngResponse(response);
                    FillAllIngList(response, allIngredients);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to fetch all ingredients");
                }
            }
            return allIngredients;
        }

        /// <summary>
        /// Creates a new user in the backend API. To be used on sign in. If user exists, no new user is created.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> to create in the backend.</param>
        /// <returns>A boolean value indicating whether the user creation was successful. The user may already
        /// exist in the backend API but this is not considered a failure.
        /// </returns>
        public async Task<bool> CreateUser(IUser user)
        {
            bool success = false;

            //Create request
            string url = config.BackendUrl + config.Create_User_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //Send request and get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);

                success = await ValidateUserCreation(response);

            }
            catch (Exception)
            {
                success = false;
            }


            return success;
        }

        /// <summary>
        /// Validates the response from the backend API after a user creation attempt.
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        /// <returns>A boolean value indicating whether the response was valid.</returns>
        private async Task<bool> ValidateUserCreation(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Created;
        }

        /// <summary>
        /// Validates the response from the backend API when fetching all ingredients. Throws
        /// exception on failure.
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateAllIngResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Fills the list of <see cref="Ingredient"/> objects with data from the backend API response.
        /// </summary>
        /// <param name="response">The HTTP response containing the ingredient data.</param>
        /// <param name="allIng">The list to populate with <see cref="Ingredient"/> objects.</param>
        private static async void FillAllIngList(HttpResponseMessage response, List<Ingredient> allIng)
        {
            // Read the response content as a string
            string body = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            var jsonBody = JsonDocument.Parse(body);

            // Access the root object directly
            JsonElement root = jsonBody.RootElement;

            // Process common ingredients
            if (root.TryGetProperty("common_ingredients", out JsonElement commonIngredients))
            {
                foreach (JsonElement item in commonIngredients.EnumerateArray())
                {
                    string? name = item.GetProperty("name").GetString();
                    string? type = item.GetProperty("type").GetString();
                    allIng.Add(new Ingredient(name, type));
                }
            }

            // Process custom ingredients
            if (root.TryGetProperty("custom_ingredients", out JsonElement customIngredients))
            {
                foreach (JsonElement item in customIngredients.EnumerateArray())
                {
                    string? name = item.GetProperty("name").GetString();
                    string? type = item.GetProperty("type").GetString();
                    allIng.Add(new Ingredient(name, type, true));
                }
            }

            // Sort ingredients alphabetically by name
            allIng.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// Retrieves the user's lists of ingredients.
        /// This method creates two predefined ingredient lists: Grocery and Pantry, 
        /// each containing a set of ingredients.
        /// </summary>
        /// <param name="user">The user for whom the lists are retrieved.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of user lists.</returns>
        public async Task<List<UserList>> GetMyLists(IUser user)
        {
            // Only make api call if needed (otherwise use cached variable)
            if (MyListsNewCall())
            {
                lastMyListsCall = DateTime.Now;
                myLists?.Clear();
                myLists = new List<UserList>();

                // Create request
                string url = config.BackendUrl + config.Get_My_Lists_Endpoint;
                string accessToken = user.GetAccessToken();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Get response
                try
                {
                    HttpResponseMessage response = await HttpClient.SendAsync(request);
                    
                    ValidateGetMyListsResponse(response);

                    FillMyLists(response, myLists);

                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to fetch your lists!");
                }
            }

            return await Task.FromResult(myLists);
        }

        /// <summary>
        /// Validates the response from the backend API when fetching user's lists. 
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateGetMyListsResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Fills in a list of <see cref="UserList"/> based on the response
        /// </summary>
        /// <param name="response">The HTTP response parse.</param>
        /// <param name="myLists">The list to be filled.</param>
        private static async void FillMyLists(HttpResponseMessage response, List<UserList> myLists)
        {
            // Read JSON from response
            var jsonString = await response.Content.ReadAsStringAsync();

            // Parse JSON as a document to access elements directly
            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            // Clear existing data in myLists
            myLists.Clear();


            // Traverse JSON to extract lists and ingredients
            foreach (var listItem in root.EnumerateArray())
            {
                string listName = listItem.GetProperty("list_name").GetString();
                List<Ingredient> ingredients = new List<Ingredient>();

                UserList currList = new UserList(listName, ingredients);

                foreach (var ingredientItem in listItem.GetProperty("ingredients").EnumerateArray())
                {
                    string name = ingredientItem.GetProperty("ingredient_name").GetString();
                    string type = ingredientItem.GetProperty("ingredient_type").GetString();
                    float amount = ingredientItem.GetProperty("amount").GetSingle();
                    string unit = ingredientItem.GetProperty("unit").GetString();
                    bool isCustom = ingredientItem.GetProperty("is_custom_ingredient").GetBoolean();

                    currList.AddIngToList(new Ingredient(name, type, amount, unit, isCustom));
                }

                // Add the list to myLists
                myLists.Add(currList);
            }

        }

        /// <summary>
        /// Adds an <see cref="Ingredient"/> to a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is adding.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be added.</param>
        /// <param name="listName">The name of the list to add to</param>
        /// <returns>A bool indicating whether addition was successfull.</returns>
        public async Task<bool> AddIngredientToList(IUser user, Ingredient ingredient, string listName)
        {
            // bool to indicate success
            bool added = false;

            // Create request
            string url = config.BackendUrl + config.Add_Ing_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var body = new
            {
                list_name = listName,
                ingredient = ingredient.GetName(),
                amount = ingredient.GetAmount(),
                unit = ingredient.GetUnit(),
                is_custom_ingredient = ingredient.IsCustom()
            };

            string jsonBody = JsonSerializer.Serialize(body);

            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateAddIngToList(response);
                added = true;
                AddToMyLists(listName, ingredient);
            }
            catch (Exception)
            {
                added = false;
            }

            return await Task.FromResult(added);
        }

        /// <summary>
        /// Validates the response from the backend API when adding an ingredient to a list
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateAddIngToList(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Returns a list of strings containing the measurement units
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> for authentication.</param>
        /// <returns>
        /// List of strings with the allowed measurement units
        /// </returns>
        public async Task<List<string>> GetAllMeasurements(IUser user)
        {
            // Only make the api call if needed (otherwise return the cached variable)
            if (AllUnitsNewCall())
            {
                lastAllUnitsCall = DateTime.Now;
                allUnits?.Clear();
                allUnits = new List<string>();

                // Create request
                string url = config.BackendUrl + config.Get_Measurements_Endpoint;
                string accessToken = user.GetAccessToken();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Get response
                try
                {
                    HttpResponseMessage response = await HttpClient.SendAsync(request);
                    ValidateGetAllMeasurements(response);
                    FillMeasurementList(response, allUnits);

                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to add fetch units!");
                }
            }

            return await Task.FromResult(allUnits);
        }


        /// <summary>
        /// Validates the response from the backend API when getting all measurements
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateGetAllMeasurements(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Fills in a list of strings of units based on the response
        /// </summary>
        /// <param name="response">The HTTP response parse.</param>
        /// <param name="units">The list of units to be filled.</param>
        private static async void FillMeasurementList(HttpResponseMessage response, List<string> units)
        {
            // Read JSON from response
            var jsonString = await response.Content.ReadAsStringAsync();

            // Parse JSON as a document to access elements directly
            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            // Clear existing data in units list
            units.Clear();

            // Traverse JSON to extract units
            foreach (var unitItem in root.EnumerateArray())
            {
                // Get the unit string
                string unit = unitItem.GetProperty("unit").GetString();

                // Add unit to the list
                units.Add(unit);
            }

        }

        /// <summary>
        /// Returns true if we need to fetch all ingredients again
        /// </summary>
        /// <returns>True if we need to fetch ingredients again, false if not</returns>
        private bool AllIngNewCall()
        {
            bool newCall = false;

            if(allIngredients == null || allIngredients?.Count == 0)
            {
                newCall = true;
            }

            if(lastAllIngCall != null)
            {
                TimeSpan diff = DateTime.Now - lastAllIngCall;
                TimeSpan threshold = TimeSpan.FromMinutes(5);

                if (diff > threshold)
                {
                    newCall = true;
                }
            }

            return newCall;
        }

        /// <summary>
        /// Returns true if we need to fetch all units again
        /// </summary>
        /// <returns>True if we need to fetch units again, false if not</returns>
        private bool AllUnitsNewCall()
        {
            bool newCall = false;

            if (allUnits == null || allUnits?.Count == 0)
            {
                newCall = true;
            }

            if (lastAllUnitsCall != null)
            {
                TimeSpan diff = DateTime.Now - lastAllUnitsCall;
                TimeSpan threshold = TimeSpan.FromMinutes(5);

                if (diff > threshold)
                {
                    newCall = true;
                }
            }

            return newCall;
        }

        /// <summary>
        /// Returns true if we need to fetch user's lists again
        /// </summary>
        /// <returns>True if we need to fetch user's lists again, false if not</returns>
        private bool MyListsNewCall()
        {
            bool newCall = false;

            if (myLists == null || myLists?.Count == 0)
            {
                newCall = true;
            }

            if (lastMyListsCall != null)
            {
                TimeSpan diff = DateTime.Now - lastMyListsCall;
                TimeSpan threshold = TimeSpan.FromMinutes(5);
                if (diff > threshold)
                {
                    newCall = true;
                }
            }

            return newCall;
        }

        /// <summary>
        /// Adds an ingredient to myList cached variable
        /// </summary>
        /// <param name="listName">Name of list to be added to</param>
        /// <param name="ingredient"><see cref="Ingredient"></see> to add to list</param>
        private void AddToMyLists(string listName, Ingredient ingredient)
        {
            UserList targetList = null;
            bool found = false;
            for (int i = 0; i < myLists.Count && !found; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    targetList = myLists[i];
                    found = true;
                    targetList.AddIngToList(ingredient.CopyIngredient());
                }
            }
        }


        /// <summary>
        /// Removes an <see cref="Ingredient"/> from a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be removed.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public async Task<bool> RemIngredientFromList(IUser user, Ingredient ingredient, string listName)
        {
            bool removed = false;

            // Create request
            string url = config.BackendUrl + config.Rem_Ing_Endpoint;
            url = url + $"?ingredient={ingredient.GetName()}&is_custom_ingredient={ingredient.IsCustom()}" +
                  $"&list_name={listName}&unit={ingredient.GetUnit()}";
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateRemoveIngredient(response);
                removed = true;
                RemFromMyLists(listName, ingredient);
            }
            catch (Exception)
            {
                removed = false;
            }

            return removed;
        }

        /// <summary>
        /// Validates the response from the backend API when removing ingredient
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateRemoveIngredient(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Removes an ingredient from myList cached variable
        /// </summary>
        /// <param name="listName">Name of list to be removed from</param>
        /// <param name="ingredient"><see cref="Ingredient"></see> to remove from list</param>
        private void RemFromMyLists(string listName, Ingredient ingredient)
        {
            UserList targetList = null;
            bool found = false;
            for (int i = 0; i < myLists.Count && !found; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    targetList = myLists[i];
                    found = true;
                    targetList.RemIngFromList(ingredient);
                }
            }
        }

        /// <summary>
        /// Edits the amount and/or unit <see cref="Ingredient"/> in <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing.</param>
        /// <param name="oldIng">The <see cref="Ingredient"/> to be edited.</param>
        /// <param name="newIng">The <see cref="Ingredient"/> with updated amount and unit.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether edit was successfull.</returns>
        public async Task<bool> SetIngredient(IUser user, Ingredient oldIng, Ingredient newIng, string listName)
        {
            bool edited = false;

            // Create request
            string url = config.BackendUrl + config.Move_Ing_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var body = new
            {
                old_list_name = listName,
                old_ingredient = oldIng.GetName(),
                old_amount = oldIng.GetAmount(),
                old_unit = oldIng.GetUnit(),
                old_is_custom_ingredient = oldIng.IsCustom(),
                new_list_name = listName,
                new_ingredient = newIng.GetName(),
                new_amount = newIng.GetAmount(),
                new_unit = newIng.GetUnit(),
                new_is_custom_ingredient = newIng.IsCustom()
            };

            string jsonBody = JsonSerializer.Serialize(body);

            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateSetIngredient(response);
                edited = true;
                SetIngMyList(oldIng, newIng, listName);
            }
            catch (Exception)
            {
                edited = false;
            }


            return edited;
        }

        /// <summary>
        /// Validates the response from the backend API when editing ingredient
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateSetIngredient(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Edits amount and/or unit of an ingredient from myList cached variable
        /// </summary>
        /// <param name="oldIng"><see cref="Ingredient"></see> to edit</param>
        /// <param name="newIng"> New <see cref="Ingredient"></see> with new amount and/or unit</param>
        /// <param name="listName">Name of list to be used</param>
        private void SetIngMyList(Ingredient oldIng, Ingredient newIng, string listName)
        {
            bool isSet = false;
            for (int i = 0; i < myLists.Count && !isSet; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    myLists[i].EditIngredientInList(oldIng, newIng);
                    isSet = true;
                }
            }
        }

        /// <summary>
        /// Removes a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing a list.</param>
        /// <param name="listName">The name of the list to be deleted</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public async Task<bool> DeleteList(IUser user, string listName)
        {
            bool deleted = false;

            // Create request
            string url = config.BackendUrl + config.Del_List_Endpoint;
            url = url.Replace("{list_name}", listName);
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateDeleteList(response);
                deleted = DeleteCachedList(listName);
            }
            catch (Exception)
            {
                deleted = false;
            }

            return deleted;
        }

        /// <summary>
        /// Validates the response from the backend API when deleting a list
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateDeleteList(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Deletes a list from myList cached variable
        /// </summary>
        /// <param name="listName">Name of list to be deleted</param>
        private bool DeleteCachedList(string listName)
        {
            bool deleted = false;
            for (int i = 0; i < myLists.Count && !deleted; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    myLists.RemoveAt(i);
                    deleted = true;
                }
            }
            return deleted;
        }

        /// <summary>
        /// Creates a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="listName">The name of the list to be created</param>
        /// <returns>A bool indicating whether creation was successfull.</returns>
        public async Task<bool> CreateList(IUser user, string listName)
        {
            bool created = false;

            // Create request
            string url = config.BackendUrl + config.Create_List_Endpoint;
            url = url.Replace("{list_name}", listName);
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateCreateList(response);     
                created = CreateCachedList(listName);
            }
            catch (Exception)
            {
                created = false;
            }

            return created;
        }

        /// <summary>
        /// Validates the response from the backend API when creating a list
        /// </summary>
        /// <param name="response">The HTTP response to validate.</param>
        private static void ValidateCreateList(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Creates a list in myList cached variable
        /// </summary>
        /// <param name="listName">Name of list to be created</param>
        private bool CreateCachedList(string listName)
        {
            bool created = false;

            bool alreadyExists = false;

            for (int i = 0; i < myLists.Count && !alreadyExists; i++)
            {
                alreadyExists = (myLists[i].GetListName() == listName);
            }

            if (!alreadyExists)
            {
                List<Ingredient> emptyIngs = new List<Ingredient>();
                UserList newList = new UserList(listName, emptyIngs);
                myLists.Add(newList);
                created = true;
            }

            return created;
        }

        /// <summary>
        /// Moves an ingredient from one list to another
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="currListName">The name of the original list where the ingredient is from</param>
        /// <param name="newListName">The name of the new list where the ingredient is to be moved</param>
        /// /// <param name="ingredient">The <see cref="Ingredient"></see> to be moved</param>
        /// <returns>A bool indicating whether moving was successfull.</returns>
        public async Task<bool> MoveIngredient(IUser user, string currListName, string newListName, Ingredient ingredient)
        {
            bool moved = false;

            // Create request
            string url = config.BackendUrl + config.Move_Ing_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var body = new
            {
                old_list_name = currListName,
                old_ingredient = ingredient.GetName(),
                old_amount = ingredient.GetAmount(),
                old_unit = ingredient.GetUnit(),
                old_is_custom_ingredient = ingredient.IsCustom(),
                new_list_name = newListName,
                new_ingredient = ingredient.GetName(),
                new_amount = ingredient.GetAmount(),
                new_unit = ingredient.GetUnit(),
                new_is_custom_ingredient = ingredient.IsCustom()
            };

            string jsonBody = JsonSerializer.Serialize(body);

            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                moved = true;
                MoveCachedIngredient(ingredient, currListName, newListName);
            }
            catch (Exception)
            {
                moved = false;
            }

            return moved;
        }

        /// <summary>
        /// Moves the ingredient from list to list in cache
        /// </summary>
        /// <param name="toMove"> Ingredient to move</param>
        /// <param name="fromList"> Original list</param>
        /// <param name="toList"> New list </param>
        private void MoveCachedIngredient(Ingredient toMove, string fromList, string toList)
        {
            RemFromMyLists(fromList, toMove);
            AddToMyLists(toList, toMove);
        }

        /// <summary>
        /// Method to rename a list
        /// </summary>
        /// <param name="user"> The user who is renaming lists</param>
        /// <param name="currListName"> Current name of the list to change </param>
        /// <param name="newListName"> New name for the list </param>
        /// <returns></returns>
        public async Task<bool> RenameList(IUser user, string currListName, string newListName)
        {
            bool renamed = false;

            // Create request
            string url = config.BackendUrl + config.Rename_List_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var body = new
            {
                old_list_name = currListName,
                new_list_name = newListName
            };

            string jsonBody = JsonSerializer.Serialize(body);

            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                RenameCacheList(currListName, newListName);
                renamed = true;
            }
            catch (Exception)
            {
                renamed = false;
            }

            return renamed;
        }

        /// <summary>
        /// Renames the list in the cached response
        /// </summary>
        /// <param name="currListName"> The list to rename </param>
        /// <param name="newListName"> The new name for list </param>
        private void RenameCacheList(string currListName, string newListName)
        {
            bool renamed = false;

            for (int i = 0; i < myLists.Count && !renamed; i++)
            {
                if (myLists[i].GetListName() == currListName)
                {
                    myLists[i].SetListName(newListName);
                    renamed = true;
                }
            }
        }

        /// <summary>
        /// Method to create a custom ingredient
        /// </summary>
        /// <param name="user"> User creating the ingredient</param>
        /// <param name="customIng"> The ingredient to be created </param>
        /// <returns> True on success, false on failure </returns>
        public async Task<bool> CreateCustomIngredient(IUser user, Ingredient customIng)
        {
            bool created = false;

            // Create request
            string url = config.BackendUrl + config.Create_Custom_Ing_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var body = new
            {
                ingredient = customIng.GetName(),
                type = customIng.GetIngType()
            };

            string jsonBody = JsonSerializer.Serialize(body);

            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                allIngredients.Add(customIng);
                allIngredients.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));
                created = true;
            }
            catch (Exception)
            {
                created = false;
            }

            return created;
        }

        /// <summary>
        /// Backend method to delete a custom ingredient
        /// </summary>
        /// <param name="user"> User who is removing ingredient </param>
        /// <param name="ingredient"> The ingredient to be removed</param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomIngredient(IUser user, Ingredient ingredient)
        {
            bool deleted = false;

            // Create request
            string url = config.BackendUrl + config.Delete_Custom_Ing_Endpoint;
            url = url.Replace("{ingredient}", ingredient.GetName());
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                for (int i = 0; i < allIngredients.Count; i++)
                {
                    Ingredient curr = allIngredients[i];
                    if (curr.GetName() == ingredient.GetName() &&
                        curr.GetIngType() == ingredient.GetIngType() &&
                        curr.IsCustom() == ingredient.IsCustom())
                    {
                        allIngredients.Remove(curr);
                        deleted = true;
                    }
                }
                allIngredients.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));

                for (int i = 0; i < myLists?.Count; i++)
                {
                    myLists[i].CascadeDelCustomIng(ingredient);
                }
            }
            catch (Exception)
            {
                deleted = false;
            }

            return deleted;
        }
    }

}
