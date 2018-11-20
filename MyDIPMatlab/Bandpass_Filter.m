function [G] = Bandpass_Filter(F,d,w)
%Low_Pass_Filter 低通滤波器
%参数 F 傅里叶变换后的矩阵
%参数 d 截止频率
% 坐标
[P,Q] = size(F);
if P>Q
    Max = P;
else
    Max = Q;
end
u = ones(Max,1) * (1:Max);
u = u(1:P,1:Q);
v = (1:Max)' * ones(1,Max);
v = v(1:P,1:Q);
% 计算
D = ((u-P/2).^2 + (v-Q/2).^2).^0.5;
% 低通滤波器阈值
H = ones(P,Q);
H((D > d - w/2 & D < d + w/2)) = 0;
G = H .* F;
figure;
T=log(F+1);                 % 进行对数变换 低通模糊 高通锐化
T = real(T);                 % 傅里叶变换后是复数，计算复数的模
imshow(T,[]);
end