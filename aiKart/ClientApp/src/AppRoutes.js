import { Home } from "./components/Home";
import ManageDecks from "./features/deck/ManageDecks";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/decks',
    element: <ManageDecks />
  }
];

export default AppRoutes;
