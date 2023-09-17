import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { addDeck } from "../features/deck/decksSlice";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
  Button,
} from "reactstrap";
import { Link } from "react-router-dom";
import CreateDeckModal from "./deck/CreateDeckModal";
import "./NavMenu.css";

const NavMenu = () => {
  const dispatch = useDispatch();

  const [collapsed, setCollapsed] = useState(true);
  const [showCreateDeckForm, setShowCreateDeckForm] = useState(false);
  const [newDeckName, setNewDeckName] = useState("");
  const [newDeckDescription, setNewDeckDescription] = useState("");

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  };

  const toggleCreateDeckForm = () => {
    setShowCreateDeckForm(!showCreateDeckForm);
  };

  const saveDeck = (event) => {
    event.preventDefault();
    if (newDeckName && newDeckDescription) {
      dispatch(addDeck({ name: newDeckName, description: newDeckDescription }));
      setNewDeckName("");
      setNewDeckDescription("");
      toggleCreateDeckForm();
    }
  };

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-dark bg-dark mb-3">
        <NavbarBrand tag={Link} to="/">
          aiKart
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse
          className="d-sm-inline-flex flex-sm-row-reverse"
          isOpen={!collapsed}
          navbar
        >
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-white" to="/">
                Home
              </NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-white" to="/decks">
                Manage Decks
              </NavLink>
            </NavItem>
            <NavItem>
              <Button color="primary" onClick={toggleCreateDeckForm}>
                Create Deck
              </Button>
            </NavItem>
          </ul>
        </Collapse>
        <CreateDeckModal
          isOpen={showCreateDeckForm}
          toggle={toggleCreateDeckForm}
          saveDeck={saveDeck}
          newDeckName={newDeckName}
          setNewDeckName={setNewDeckName}
          newDeckDescription={newDeckDescription}
          setNewDeckDescription={setNewDeckDescription}
        />
      </Navbar>
    </header>
  );
};

export default NavMenu;
