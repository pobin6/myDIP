function [G,uk,vk] = Notch_Filter(F,D0,n)
%Low_Pass_Filter ��ͨ�˲���
%���� F ����Ҷ�任��ľ���
%���� d ��ֹƵ��
% ����
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
% �����ݲ�
Pix_max = max(max(real(F(1:P/2,1:Q/2))));
[uk1,vk1] = find((real(F(1:P/2,1:Q/2))== Pix_max));
uk = [P/2-uk1;uk1-P/2];
vk = [Q/2-vk1;Q/2-vk1];
H = ones(P,Q);
for i=1:n
    Dk = ((u-P/2-uk(i)).^2 + (v-Q/2-vk(i)).^2).^0.5;
    D_k = ((u-P/2+uk(i)).^2 + (v-Q/2+vk(i)).^2).^0.5;
    % ��ͨ�˲�����ֵ
    H = H.*(1./(1+(D0./Dk).^4)).*(1./(1+(D0./D_k).^4));
end
G = H .* F;
end