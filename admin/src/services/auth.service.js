import createApiClient from "./api.service.js";
class AuthService {
    constructor(baseUrl = "http://localhost:5086/api/v1/auth") {
        this.api = createApiClient(baseUrl);
        this.token = sessionStorage.getItem('token');
    }
    setToken(token) {
        this.token = token;
        sessionStorage.setItem('token', token);
    }
    async login(data) {
        const response = await this.api.post("/loginAdmin", data);
        if (response && response.status === 200) {
            this.setToken(response.data.token);
        }
        return response;
    }
    async profileAdmin(){
        try {
            return (await this.api.get("/profileAdmin", {
                headers: {
                    'Authorization': `Bearer ${this.token}`
                }
            }));
        }catch (e) {
            console.log(e.data);
        }
    }
    async logout(){
        console.log(this.token)
        return (await this.api.get("/logout",{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new AuthService();