import Home from "./components/Home";
import ManageDecks from "./components/deck/ManageDecks";
import DeckView from "./components/deck/DeckView";
import DeckPractice from "./components/DeckPractice"; 
const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/decks',
    element: <ManageDecks />
  },
  {
    path: '/decks/:deckName',
    element: <DeckView />
  },
  {
    path: '/practice',
    element: <DeckPractice />
  },
  {
    path: '/practice/:deckName',
    element: <DeckPractice />
  }
];

export default AppRoutes;
