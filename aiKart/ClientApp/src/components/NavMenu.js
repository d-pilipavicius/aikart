import React, { useState, useEffect } from "react";
import {
  Navbar,
  NavbarBrand,
  NavbarToggler,
  Collapse,
  NavItem,
  NavLink,
  Nav,
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
} from "reactstrap";
import { Link } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { FaUser, FaUserCircle } from "react-icons/fa";
import { IoStatsChartSharp } from "react-icons/io5";
import "./NavMenu.css";
import { resetUserDecks } from "../app/state/user/userDecksSlice";
import { logout } from "../app/state/user/usersSlice";

const NavMenu = ({ disableNavMenu }) => {
  const [collapsed, setCollapsed] = useState(true);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const dispatch = useDispatch();
  const navigate = useNavigate();

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

  const toggleDropdown = () => {
    setDropdownOpen(!dropdownOpen);
  };

  const handleLogout = () => {
    navigate(`/`);
    dispatch(resetUserDecks());
    dispatch(logout());
  };

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-dark bg-dark mb-3">
        <NavbarBrand>aiKart</NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        {!disableNavMenu && (
          <Collapse isOpen={!collapsed} navbar>
            <Nav className="ms-auto" navbar>
              <NavItem>
                <NavLink
                  tag={Link}
                  className="text-white"
                  to="/home"
                  activeClassName="active"
                >
                  Home
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink
                  tag={Link}
                  className="text-white"
                  to="/store"
                  activeClassName="active"
                >
                  Store
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink
                  tag={Link}
                  className="text-white"
                  to="/decks"
                  activeClassName="active"
                >
                  Manage Decks
                </NavLink>
              </NavItem>
              <NavItem>
                <Dropdown isOpen={dropdownOpen} toggle={toggleDropdown}>
                  <DropdownToggle className="nav-link text-white" caret>
                    <FaUserCircle size={25} /> {currentUser && currentUser.name}
                  </DropdownToggle>
                  <DropdownMenu end>
                    <DropdownItem tag={Link} to="/Statistics">
                      <IoStatsChartSharp /> Statistics
                    </DropdownItem>
                    <DropdownItem onClick={handleLogout}>
                      <FaUser className="mr-2" /> Log out
                    </DropdownItem>
                  </DropdownMenu>
                </Dropdown>
              </NavItem>
            </Nav>
          </Collapse>
        )}
      </Navbar>
    </header>
  );
};

export default NavMenu;
