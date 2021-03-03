$("#registerForm").validate({
    rules: {
        remote: "VerifyEmail",
        type: "post"
    }
});