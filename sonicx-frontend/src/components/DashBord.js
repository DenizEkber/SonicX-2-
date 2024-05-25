import { Route, Routes } from "react-router-dom";
import Login from "./Login";
import Register from "./Register";
import Main from "./Main";
import HomePage from "../pages/Home";

export default function Dashboard(){
    return(
        <div>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/main" element={<Main />} />
                <Route path="/" element={<HomePage />} />
            </Routes>
        </div>
    )
}