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
v = ones(Max,1) * (1:Max);
v = v(1:P,1:Q);
u = (1:Max)' * ones(1,Max);
u = u(1:P,1:Q);
% 计算
D = ((u-P/2).^2 + (v-Q/2).^2).^0.5;
% 低通滤波器阈值
H = ones(P,Q);
H((D > d - w/2 & D < d + w/2)) = 0;
G = H .* F;
end