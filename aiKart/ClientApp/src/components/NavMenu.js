import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { addDeck } from "../features/deck/decksSlice";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
  Button,
  Form,
  Input,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
} from "reactstrap";
import { Link } from "react-router-dom";

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

  const saveDeck = (e) => {
    e.preventDefault();
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
        <Modal isOpen={showCreateDeckForm} toggle={toggleCreateDeckForm}>
          <ModalHeader toggle={toggleCreateDeckForm}>
            Create New Deck
          </ModalHeader>
          <ModalBody>
            <Form onSubmit={saveDeck}>
              <Input
                type="text"
                placeholder="Deck Name"
                value={newDeckName}
                onChange={(e) => setNewDeckName(e.target.value)}
              />
              <Input
                type="text"
                placeholder="Deck Description"
                value={newDeckDescription}
                onChange={(e) => setNewDeckDescription(e.target.value)}
                className="mt-2"
              />
            </Form>
          </ModalBody>
          <ModalFooter>
            <Button color="success" onClick={saveDeck}>
              Save
            </Button>
            <Button color="secondary" onClick={toggleCreateDeckForm}>
              Cancel
            </Button>
          </ModalFooter>
        </Modal>
      </Navbar>
    </header>
  );
};

export default NavMenu;
