import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addUser, setCurrentUser } from "../app/state/user/usersSlice";
import {
  Card,
  CardBody,
  Form,
  FormGroup,
  Container,
  Input,
  Button,
} from "reactstrap";

const UserLogin = () => {
  const [username, setUsername] = useState("");
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogin = async () => {
    const newUser = { name: username };

    dispatch(addUser(newUser))
      .then((response) => {
        dispatch(setCurrentUser(response.payload));
        console.log(response.payload);
        navigate(`/home`);
      })
      .catch((error) => {
        console.error("User login failed", error);
      });
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      e.preventDefault();
      handleLogin();
    }
  };

  return (
    <Container className="d-flex">
      <Card className="border-0 mx-auto">
        <CardBody>
          <h1 className="mb-4">Login to aiKart</h1>
          <Form>
            <FormGroup>
              <Input
                type="text"
                id="username"
                placeholder="Enter your username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                onKeyDown={handleKeyDown}
              />
            </FormGroup>
            <Container className="d-flex justify-content-center">
              <Button color="primary" onClick={handleLogin} className="mt-3">
                Login
              </Button>
            </Container>
          </Form>
        </CardBody>
      </Card>
    </Container>
  );
};

export default UserLogin;
