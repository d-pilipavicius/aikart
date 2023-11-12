import React from "react";
import { Container } from "reactstrap";
import NavMenu from "./NavMenu";
import { useLocation } from "react-router-dom";

const Layout = ({ children }) => {
  const location = useLocation();

  const isUserLoginPage = location.pathname === "/";
  console.log(isUserLoginPage);

  return (
    <div>
      <NavMenu disableNavMenu={isUserLoginPage} />
      <Container tag="main">{children}</Container>
    </div>
  );
};

export default Layout;
