using System;
using System.Collections.Generic;
using RequestQueueDemo.Core.Domain;

namespace RequestQueueDemo.App.Features.Breeds
{
    public interface IBreedsView
    {
        public void Show();
        public void Hide();
        public void ShowListLoader();
        public void HideListLoader();
        public void RenderBreeds(IReadOnlyList<Breed> breeds, Action<string> onBreedClick);
        public void ShowFactsLoader(string breedId);
        public void HideFactsLoader(string breedId);
        public void ShowPopup(string title, string body);
    }
}
