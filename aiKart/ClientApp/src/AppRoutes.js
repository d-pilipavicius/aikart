import { Home } from "./components/Home";
import ManageDecks from "./features/deck/ManageDecks";
import DeckView from "./components/deck/DeckView"; 
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
  }
];

export default AppRoutes;
