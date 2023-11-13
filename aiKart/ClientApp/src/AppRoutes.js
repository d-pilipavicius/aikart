import UserLogin from "./components/UserLogin";
import Home from "./components/Home";
import ManageDecks from "./components/deck/ManageDecks";
import DeckView from "./components/deck/DeckView";
import DeckPractice from "./components/DeckPractice";
import Store from "./components/Store";

const AppRoutes = [
  {
    index: true,
    element: <UserLogin />,
  },
  {
    path: "/home",
    element: <Home />,
  },
  {
    path: "/decks",
    element: <ManageDecks />,
  },
  {
    path: "/decks/:deckId",
    element: <DeckView />,
  },
  {
    path: "/practice",
    element: <DeckPractice />,
  },
  {
    path: "/practice/:deckId",
    element: <DeckPractice />,
  },
  {
    path: "/store",
    element: <Store />,
  }
];

export default AppRoutes;
