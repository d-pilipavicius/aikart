import React, { useState, useEffect } from "react";
import {
  Navbar,
  NavbarBrand,
  NavbarToggler,
  Collapse,
  NavItem,
  NavLink,
  Nav,
} from "reactstrap";
import { Link } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../app/state/user/usersSlice";
import { resetUserDecks } from "../app/state/user/userDecksSlice";

import "./NavMenu.css";

const NavMenu = ({ disableNavMenu }) => {
  const [collapsed, setCollapsed] = useState(true);
  const dispatch = useDispatch();

  const currentUser = useSelector((state) => state.users.currentUser);

  useEffect(() => {
    if (disableNavMenu) {
      setCollapsed(true);
    }
  }, [disableNavMenu]);

  const toggleNavbar = () => {
    if (!disableNavMenu) {
      setCollapsed(!collapsed);
    }
  };

  const handleLogout = () => {
    dispatch(logout());
    dispatch(resetUserDecks());
  };

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-dark bg-dark mb-3">
        <NavbarBrand tag={Link} to="/">
          aiKart
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        {!disableNavMenu && (
          <Collapse isOpen={!collapsed} navbar>
            <Nav className="ml-auto" navbar>
              <NavItem>
                <NavLink tag={Link} className="text-white" to="/home">
                  Home
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-white" to="/store">
                  Store
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-white" to="/decks">
                  Manage Decks
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-white" to="/" onClick={handleLogout}>
                  Log out
                </NavLink>
              </NavItem>
            </Nav>
            <Nav className="ms-auto" navbar>
              <NavItem className="text-white">
                Logged in as {currentUser && currentUser.name}
              </NavItem>
            </Nav>
          </Collapse>
        )}
      </Navbar>
    </header>
  );
};

export default NavMenu;
