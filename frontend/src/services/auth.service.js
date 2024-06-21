import createApiClient from "./api.service.js";
class AuthService {
    constructor(baseUrl = "http://localhost:5086/api/v1/auth") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    setToken(token) {
        this.token = token;
        localStorage.setItem('token', token);
    }
    async signup(data) {
        return (await this.api.post("/register", data));
    }
    async login(data) {
        const response = await this.api.post("/login", data);
        if (response && response.status === 200) {
            this.setToken(response.data.token);
        }
        return response;
    }
    async profile(){
        try {
            return (await this.api.get("/profile", {
                headers: {
                    'Authorization': `Bearer ${this.token}`
                }
            }));
        }catch (e) {
            console.log(e.data);
        }
    }
    async logout(){
        return (await this.api.get("/logout",{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async switch(data){
        return (await this.api.post("/switch",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new AuthService();