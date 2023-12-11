import React, { useEffect } from "react";
import { Container } from "reactstrap";
import NavMenu from "./NavMenu";
import { useLocation, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";

const Layout = ({ children }) => {
  const location = useLocation();
  const navigate = useNavigate();
  const isUserLoginPage = location.pathname === "/";
  const user = useSelector((state) => state.users.currentUser);

  useEffect(() => {
    if (!isUserLoginPage && !user) {
      navigate('/');
    }
  }, [isUserLoginPage, navigate, user]);

  return (
    <div>
      <NavMenu disableNavMenu={isUserLoginPage} />
      <Container tag="main">{children}</Container>
    </div>
  );
};

export default Layout;
